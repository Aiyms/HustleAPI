using Hustle.Repositories;

namespace Hustle.Interfaces
{
    public interface IAuthRepository
    {
        public ApiResponse<string> Register(UserRegistration input);
        public ApiResponse<string> SignIn(UserLogin input);
        public ApiResponse<string> ChangePassword(UserLogin input);
    }
}