
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

    public async Task<int> RegisterAsync(User user)
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

    public async Task<User?> GetByEmailAsync(string email)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Id, Username, Email, PasswordHash, CreatedAt
                         FROM [User]
                         WHERE Email = @Email";

        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);

        await connection.OpenAsync();

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Username = reader.GetString(reader.GetOrdinal("Username")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
        };
    }
}
