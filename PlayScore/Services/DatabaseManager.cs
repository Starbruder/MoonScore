using MoonScore.Models;
using System.Data.SQLite;

namespace MoonScore.Services;

public sealed class DatabaseManager(SQLiteConnection connection) : IService
{
    public void ConnectToDatabase()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            return;
        }

        try
        {
            connection.Open();
            Console.WriteLine("Database connected.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task AddGameToSpieleTableAsync(GameModel game)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var sql = @"
        INSERT INTO Spiele (ID, Name, ReleaseDate, Rating, MondphaseID)
        VALUES (@ID, @Name, @ReleaseDate, @Rating, @MondphaseID);";

        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@ID", game.Id);
        command.Parameters.AddWithValue("@Name", game.Name);
        command.Parameters.AddWithValue("@Rating", game.Rating);
        command.Parameters.AddWithValue("@ReleaseDate", game.Released);
        command.Parameters.AddWithValue("@MondphaseID", game.MondphaseID);

        await command.ExecuteNonQueryAsync();
    }

    public async Task AddGamesToSpieleTableAsync(ICollection<GameModel> games)
    {
        foreach (var game in games)
        {
            if (game.Rating > 0)
            {
                await AddGameToSpieleTableAsync(game);
            }
        }
    }

    public IEnumerable<KeyValuePair<string, double>> GetAverageRatingsPerMondphases()
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        var sql = @"
            SELECT m.Name, AVG(s.Rating) 
            FROM Spiele s
            JOIN Mondphasen m ON s.MondphaseID = m.Id
            GROUP BY m.Name;";

        using var command = new SQLiteCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var mondphaseName = reader.GetString(0);
            var averageRating = reader.GetDouble(1);

            yield return new KeyValuePair<string, double>(mondphaseName, averageRating);

            Console.WriteLine($"Unknown moon phase: {mondphaseName}");
        }
    }

    public IEnumerable<KeyValuePair<string, long>> GetCountOfGamesPerMoonphase()
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        var sql = @"
            SELECT m.Name, COUNT(s.Id) AS GameCount
            FROM Spiele s
            JOIN Mondphasen m ON s.MondphaseID = m.Id
            GROUP BY m.Name
            ORDER BY m.Id;";

        using var command = new SQLiteCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var mondphaseName = reader.GetString(0);
            var gameCount = reader.GetInt64(1);

            yield return new KeyValuePair<string, long>(mondphaseName, gameCount);

            Console.WriteLine($"Unknown moon phase: {mondphaseName}");
        }
    }
}
