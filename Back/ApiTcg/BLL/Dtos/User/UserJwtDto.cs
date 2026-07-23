namespace BLL.Dtos.User;

public class UserJwtDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
