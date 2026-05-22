
using DAL.Interfaces;
using Domain.Entities;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> CreateAsync(User user)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"INSERT INTO [User] (Username,Email, PasswordHash)
                       VALUES (@Username,@Email,@PasswordHash)";

        using SqlCommand command = new SqlCommand(query,connection);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteNonQueryAsync());
    }
}
