using Domain.Entities;

namespace BLL.Interfaces;

public interface IUserService
{
    Task<int> CreateAsync(User user);
}
