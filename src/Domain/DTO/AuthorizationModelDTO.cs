namespace Domain.DTO;

public class AuthorizationModelDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public bool EmailConfirmed { get; set; }
}
