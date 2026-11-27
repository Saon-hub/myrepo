using Artistry.ClassroomSolution.Model;
using MySqlConnector;

namespace Artistry.ClassroomSolution.DAL;

public class EntityTypeDAL
{
    private readonly DbConnectionFactory _connectionFactory;

    public EntityTypeDAL(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // CREATE
    public async Task<int> CreateAsync(EntityType entityType)
    {
        const string sql = @"
            INSERT INTO EntityType (Name, Description)
            VALUES (@Name, @Description);
            SELECT LAST_INSERT_ID();";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Name", entityType.Name);
        command.Parameters.AddWithValue(
            "@Description",
            (object?)entityType.Description ?? DBNull.Value);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    // UPDATE
    public async Task<bool> UpdateAsync(EntityType entityType)
    {
        const string sql = @"
            UPDATE EntityType
            SET Name = @Name,
                Description = @Description
            WHERE Id = @Id";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", entityType.Id);
        command.Parameters.AddWithValue("@Name", entityType.Name);
        command.Parameters.AddWithValue(
            "@Description",
            (object?)entityType.Description ?? DBNull.Value);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql =
            "DELETE FROM EntityType WHERE Id = @Id";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", id);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    // SELECT ALL
    public async Task<List<EntityType>> SelectAllAsync()
    {
        const string sql = @"
            SELECT Id, Name, Description
            FROM EntityType
            ORDER BY Id DESC";

        var result = new List<EntityType>();

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new EntityType
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString("Description")
            });
        }

        return result;
    }
}