using DAL.Interfaces;
using Domain.Entities;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Dapper;

namespace DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    #region Creat

        public async Task<int> RegisterAsync(User user)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"INSERT INTO [User] (Username,Email, PasswordHash)
                           VALUES (@Username,@Email,@PasswordHash)";

            return await connection.ExecuteAsync(query, user);

        }

    #endregion


    #region Read

    public async Task<User?> GetByEmailAsync(string email)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Id, Username, Email, PasswordHash, CreatedAt, IsDeleted, DeletedAt
                             FROM [User]
                             WHERE Email = @Email";

        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
    }

    public async Task<int> IsEmailTakenAsync(string email)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Count(1) FROM [User] WHERE Email = @Email";

        return await connection.ExecuteScalarAsync<int>(query, new { Email = email });
    }

    public async Task<int> IsUsernameTakenAsync(string username)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Count(1) FROM [User] WHERE Username = @Username";

        return await connection.ExecuteScalarAsync<int>(query, new { Username = username });
    }



    #endregion


    #region Update

    public async Task<int> SoftDeleteUserAsync(int userId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
            UPDATE [User]
            SET IsDeleted = 1,
                DeletedAt = GETDATE()
            WHERE Id = @UserId";

            return await connection.ExecuteAsync(query, new { UserId = userId });
        }


    #endregion


    #region Delete


        public async Task<int> HardDeleteUserAsync(int userId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"DELETE FROM [User]
                             WHERE Id = @UserId";

            return await connection.ExecuteAsync(query, new { UserId = userId });
        }


        public async Task<int> HardDeleteUserAsync(DateTime? deletedDate)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = "DELETE FROM [User] ";

            object parameters;

            if (deletedDate != null)
            {
                query += "WHERE DeletedAt <= @DeletedAt";
                parameters = new { DeletedAt = deletedDate };
            }
            else
            {
                query += "WHERE IsDeleted = 1";
                parameters = new { };
            }

            return await connection.ExecuteAsync(query, parameters);
        }

    #endregion
}
