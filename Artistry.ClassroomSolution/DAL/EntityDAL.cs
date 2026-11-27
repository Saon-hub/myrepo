using Artistry.ClassroomSolution.Model;
using MySqlConnector;

namespace Artistry.ClassroomSolution.DAL;

public class EntityDAL
{
    private readonly DbConnectionFactory _connectionFactory;

    public EntityDAL(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // CREATE
    public async Task<int> CreateAsync(Entity entity)
    {
        const string sql = @"
            INSERT INTO Entity
                (Name, Description, EntityTypeId)
            VALUES
                (@Name, @Description, @EntityTypeId);
            SELECT LAST_INSERT_ID();";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue(
            "@Description",
            (object?)entity.Description ?? DBNull.Value);
        command.Parameters.AddWithValue(
            "@EntityTypeId", entity.EntityTypeId);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Entity entity)
    {
        const string sql = @"
            UPDATE Entity
            SET Name = @Name,
                Description = @Description,
                EntityTypeId = @EntityTypeId
            WHERE Id = @Id";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", entity.Id);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue(
            "@Description",
            (object?)entity.Description ?? DBNull.Value);
        command.Parameters.AddWithValue(
            "@EntityTypeId", entity.EntityTypeId);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Entity WHERE Id = @Id";

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", id);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    // SELECT ALL
    public async Task<List<Entity>> SelectAllAsync()
    {
        const string sql = @"
            SELECT Id, Name, Description, EntityTypeId
            FROM Entity
            ORDER BY Id DESC";

        var result = new List<Entity>();

        await using var connection =
            await _connectionFactory.CreateConnectionAsync();

        await using var command = new MySqlCommand(sql, connection);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Entity
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString("Description"),
                EntityTypeId = reader.GetInt32("EntityTypeId")
            });
        }

        return result;
    }
}