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

    public void AddGameToSpieleTable(GameModel game)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
        }

        string sql = "INSERT INTO Spiele (ID, Name, Release_Date, Rating, Mondphase_ID) " +
                     "VALUES (@ID, @Name, @Release_Date, @Rating, @Mondphase_ID);";

        using (var command = new SQLiteCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@ID", game.Id);
            command.Parameters.AddWithValue("@Name", game.Name);
            command.Parameters.AddWithValue("@Rating", game.Rating);
            command.Parameters.AddWithValue("@Release_Date", game.Released);
            command.Parameters.AddWithValue("@Mondphase_ID", 0);

            command.ExecuteNonQuery();
        }
    }
}
