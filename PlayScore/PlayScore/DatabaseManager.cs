using PlayScore.Models;
using System.Data.SQLite;
using System.Globalization;

namespace PlayScore;

public sealed class DatabaseManager(SQLiteConnection connection)
{
    public static void CreateDatabase(string dbPath)
    {
        if (!System.IO.File.Exists(dbPath))
        {
            SQLiteConnection.CreateFile(dbPath);
            Console.WriteLine("Database file created.");
        }
    }

    public void CreateTable(string tableName)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        using SQLiteCommand command = new($"CREATE TABLE IF NOT EXISTS [{tableName}] (Id INTEGER PRIMARY KEY, Name TEXT);", connection);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Table Created or already exists.");
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

        string sql = "INSERT INTO Spiele (ID, Name, Release_Date, Rating, Mondphase_ID) " +
                     "VALUES (@ID, @Name, @Rating, @Release_Date, @Mondphase_ID);";

        string formattedRating = game.Rating.ToString(CultureInfo.InvariantCulture); //Format Rating so it can be inserted

        using (var command = new SQLiteCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@ID", game.Id);
            command.Parameters.AddWithValue("@Name", game.Name);
            command.Parameters.AddWithValue("@Rating", game.Rating);
            command.Parameters.AddWithValue("@Release_Date", formattedRating);
            command.Parameters.AddWithValue("@Mondphase_ID", 0);

            command.ExecuteNonQuery();
        }
    }
}
