using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Hustle.Interfaces;

namespace Hustle.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        //private readonly string _connectionString = "Host=localhost:5432;Username=postgres;Password=Alser1234;Database=postgres";

        private readonly string _connectionString = "Host=ec2-52-0-79-72.compute-1.amazonaws.com;Port=5432;Database=d85b4ci8ushl2f;Username=mkkxvzybqzukra;Password=59a766f9938a5d14fc0dfc2f8a1b389694a40d9ca0c5cdb4dd6bd71828b0b6fe";
        private readonly IConfiguration _configuration;
        
        public ApiResponse<string> Register(UserRegistration input) 
        {
           // var t = SecurePasswordHasher.Hash(input.password, 2);
            ApiResponse<string> result = new ApiResponse<string>();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
                {
                     conn.Open();
                     using (NpgsqlCommand cmd = new NpgsqlCommand())
                     {
                        cmd.Connection = conn;
                        cmd.CommandText = $"INSERT INTO USERS (email, password, role) VALUES (@email, @password, @role)";
                        cmd.Parameters.AddWithValue("email", input.email);
                        cmd.Parameters.AddWithValue("password", /*SecurePasswordHasher.Hash(input.password, 2)*/ input.password);
                        cmd.Parameters.AddWithValue("role", 1);
                        cmd.ExecuteNonQueryAsync();
                     }
                 }
                result.Data = "data in database";
            }
            catch (Exception ex) 
            {
                result.Code = -1;
                result.Data = null;
                result.Message = ex.Message;

            }
            return result;
        }

        

        public ApiResponse<string> SignIn(UserLogin input) 
        {
            ApiResponse<string> result = new ApiResponse<string>();
            var user = Authenticate(input);

            if (user != null)
            {
                var token = Generate(input);
                result.Data = token;
                return result;
            }

           // return NotFound("User not found");
            return result;
        }

        private string Authenticate(UserLogin input)
        {
            ApiResponse<string> result = new ApiResponse<string>();
            //var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
                {
                     conn.Open();
                     using (NpgsqlCommand cmd = new NpgsqlCommand())
                     {
                        cmd.Connection = conn;
                        cmd.CommandText = $"SELECT * FROM USERS s WHERE s.email = @email";
                        cmd.Parameters.AddWithValue("email", input.email);
                        cmd.ExecuteNonQueryAsync();
                     }
                 }
                result.Data = "data in database";
            }
            catch (Exception ex) 
            {
                result.Code = -1;
                result.Data = null;
                result.Message = ex.Message;

            }

            if (result.Data != null)
            {
                var user = result.Data;
                return user;
            }

            return "User is not found";
        }

        private string Generate(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.email)
              // new Claim(ClaimTypes.Role, user.role)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ApiResponse<string> ChangePassword(UserLogin input) 
        {
            ApiResponse<string> result = new ApiResponse<string>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
                {
                     conn.Open();
                     using (NpgsqlCommand cmd = new NpgsqlCommand())
                     {
                        cmd.Connection = conn;
                        cmd.CommandText = $"UPDATE USERS SET  WHERE EMAIL = @email";
                        cmd.Parameters.AddWithValue("email", input.email);
                        cmd.ExecuteNonQueryAsync();
                     }
                 }
                result.Data = "data in database";
            }
            catch (Exception ex) 
            {
                result.Code = -1;
                result.Data = null;
                result.Message = ex.Message;

            }
            return result;
        }

    }
}