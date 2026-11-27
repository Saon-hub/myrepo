using Artistry.ClassroomSolution.Model;
using MySqlConnector;

namespace Artistry.ClassroomSolution.DAL;

public class UserDAL
{
    private readonly DbConnectionFactory _connectionFactory;
    public UserDAL(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // CREATE
    public async Task<long> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users
            (
                FirstName, LastName, MiddleName,
                Comments, PhoneNumber, Email,UserTypeId,
                LoginId, CreatedBy
            )
            VALUES
            (
                @FirstName, @LastName, @MiddleName,
                @Comments, @PhoneNumber, @Email,@UserTypeId,
                @LoginId, @CreatedBy
            );

            SELECT LAST_INSERT_ID();";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName",
            (object?)user.LastName ?? DBNull.Value);
        command.Parameters.AddWithValue("@MiddleName",
            (object?)user.MiddleName ?? DBNull.Value);
        command.Parameters.AddWithValue("@Comments",
            (object?)user.Comments ?? DBNull.Value);
        command.Parameters.AddWithValue("@PhoneNumber",
            (object?)user.PhoneNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email",
            (object?)user.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@UserTypeId",
            (object?)user.UserTypeId ?? DBNull.Value);        
        command.Parameters.AddWithValue("@LoginId",
            (object?)user.LoginId ?? DBNull.Value);
        command.Parameters.AddWithValue("@CreatedBy",
            (object?)user.CreatedBy ?? DBNull.Value);

        return Convert.ToInt64(await command.ExecuteScalarAsync());
    }

    // UPDATE
    public async Task<bool> UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE Users
            SET FirstName = @FirstName,
                LastName = @LastName,
                MiddleName = @MiddleName,
                Comments = @Comments,
                PhoneNumber = @PhoneNumber,
                Email = @Email,
                UserTypeId = @UserTypeId,
                LoginId = @LoginId,
                ModifiedDate = UTC_TIMESTAMP()
            WHERE Id = @Id";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName",
            (object?)user.LastName ?? DBNull.Value);
        command.Parameters.AddWithValue("@MiddleName",
            (object?)user.MiddleName ?? DBNull.Value);
        command.Parameters.AddWithValue("@Comments",
            (object?)user.Comments ?? DBNull.Value);
        command.Parameters.AddWithValue("@PhoneNumber",
            (object?)user.PhoneNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email",
            (object?)user.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@LoginId",
            (object?)user.LoginId ?? DBNull.Value);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(long id)
    {
        const string sql = "DELETE FROM Users WHERE Id = @Id";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();
        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    // SELECT ALL
    public async Task<List<User>> SelectAllAsync()
    {
        const string sql = @"
            SELECT
                Id, FirstName, LastName, MiddleName,
                Comments, PhoneNumber, Email, LoginId,UserTypeId,
                CreatedDate, CreatedBy, ModifiedDate
            FROM Users
            ORDER BY Id DESC";

        var result = new List<User>();
        await using var connection =
            await _connectionFactory.CreateConnectionAsync();
        await using var command = new MySqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new User
            {
                Id = reader.GetInt64("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.IsDBNull(reader.GetOrdinal("LastName"))
                    ? null : reader.GetString("LastName"),
                MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName"))
                    ? null : reader.GetString("MiddleName"),
                Comments = reader.IsDBNull(reader.GetOrdinal("Comments"))
                    ? null : reader.GetString("Comments"),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                    ? null : reader.GetString("PhoneNumber"),
                Email = reader.IsDBNull(reader.GetOrdinal("Email"))
                    ? null : reader.GetString("Email"),
                LoginId = reader.IsDBNull(reader.GetOrdinal("LoginId"))
                    ? null : reader.GetString("LoginId"),
                UserTypeId = reader.IsDBNull(reader.GetOrdinal("UserTypeId"))
                    ? null : reader.GetInt64("UserTypeId"),
                CreatedDate = reader.GetDateTime("CreatedDate"),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy"))
                    ? null : reader.GetInt64("CreatedBy"),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))
                    ? null : reader.GetDateTime("ModifiedDate")
            });
        }

        return result;
    }
}