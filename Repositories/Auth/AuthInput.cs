using Microsoft.AspNetCore.Identity;

namespace Hustle
{
    public record UserRegistration(
        string email,
        string password,
        Role role
    );

    public record UserLogin(
        string email,
        string password
    );
    
    public enum Role{
        Coach = 1,
        Client = 2
    }
}