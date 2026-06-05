using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class UserCardService : IUserCardService
{
    private readonly IUserCardRepository _userCardRepository;

    public UserCardService(IUserCardRepository userCardRepository)
    {
        this._userCardRepository = userCardRepository;
    }

    public async Task<int> AddUserCardAsync(UserCard userCard)
    {
        return await _userCardRepository.AddUserCardAsync(userCard);
    }

    public async Task<int> DeleteUserCardAsync(string id)
    {
        return await _userCardRepository.DeleteUserCardAsync(id);
    }

    public async Task<UserCard?> GetByIdAsync(string id)
    {
        return await _userCardRepository.GetByIdAsync(id);
    }
}
