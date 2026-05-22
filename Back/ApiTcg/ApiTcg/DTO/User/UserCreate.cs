namespace ApiTcg.DTO.User;

public class UserCreate
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}
