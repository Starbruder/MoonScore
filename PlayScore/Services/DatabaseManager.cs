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

    public Dictionary<string, double> GetAverageRatingPerMondphase()
    {
        var averages = new Dictionary<string, double>();

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

            averages[mondphaseName] = averageRating;
        }

        return averages;
    }

    public Dictionary<string, long> GetCountOfGamesPerMoonphase()
    {
        var counts = new Dictionary<string, long>();

        try
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            var sql = @"
            SELECT m.Name, COUNT(s.Id) AS GameCount
            FROM Spiele s
            JOIN Mondphasen m ON s.MondphaseID = m.Id
            GROUP BY m.Name;";

            // Use the connection and command to execute the query
            using var command = new SQLiteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var mondphaseName = reader.GetString(0);
                var gameCount = reader.GetInt64(1);

                counts[mondphaseName] = gameCount;
            }
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            Console.WriteLine($"Error occurred: {ex.Message}");
        }

        return counts;
    }
}
