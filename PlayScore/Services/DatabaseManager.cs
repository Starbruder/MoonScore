using MoonScore.Models;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace MoonScore.Services;

public sealed class DatabaseManager(SQLiteConnection connection)
{
    private const string databaseName = "PlayScore";

    public static string GetDatabaseName() => ( databaseName + ".db" );

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

    public void AddGameToSpieleTable(GameModel game)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        var sql = "INSERT INTO Spiele (ID, Name, Release_Date, Rating, MondphaseName) " +
                     "VALUES (@ID, @Name, @Release_Date, @Rating, @MondphaseName);";

        using var command = new SQLiteCommand(sql, connection);
        command.Parameters.AddWithValue("@ID", game.Id);
        command.Parameters.AddWithValue("@Name", game.Name);
        command.Parameters.AddWithValue("@Rating", game.Rating);
        command.Parameters.AddWithValue("@Release_Date", game.Released);
        command.Parameters.AddWithValue("@MondphaseName", game.MondphaseName);

        command.ExecuteNonQuery();
    }

    public Dictionary<string, double> GetAverageRatingPerMondphase()
    {
        var averages = new Dictionary<string, double>();

        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        var sql = @"
        SELECT MondphaseName, AVG(Rating) 
        FROM Spiele 
        GROUP BY MondphaseName";

        using var command = new SQLiteCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            string mondphase = reader.GetString(0);
            double averageRating = reader.GetDouble(1);

            averages[mondphase] = averageRating;
        }

        return averages;
    }

}
