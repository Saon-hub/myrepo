using MySqlConnector;

namespace Artistry.ClassroomSolution.DAL;

public class DbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<MySqlConnection> CreateConnectionAsync()
    {
        var connectionString =
            _configuration.GetConnectionString("ArtClassDb");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "ArtClassDb connection string is missing.");

        var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync();

        return connection;
    }
}