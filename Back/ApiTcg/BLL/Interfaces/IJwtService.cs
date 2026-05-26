using Domain.Entities;

namespace BLL.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
