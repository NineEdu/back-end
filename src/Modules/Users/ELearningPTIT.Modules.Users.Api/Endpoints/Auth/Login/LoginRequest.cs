namespace ELearningPTIT.Modules.Users.Api.Endpoints.Auth.Login;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
