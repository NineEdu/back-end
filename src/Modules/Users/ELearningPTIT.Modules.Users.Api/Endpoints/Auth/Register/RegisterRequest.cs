namespace ELearningPTIT.Modules.Users.Api.Endpoints.Auth.Register;

public class RegisterRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string>? Roles { get; set; }
}
