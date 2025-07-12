using MakesEasy.Interfaces;
using MakesEasy.Models;
using Npgsql;

namespace MakesEasy.Repo;

public class MessageRepo : IMessageInterface
{
    private readonly string _connectionString;

    public MessageRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> PostMessage(MessageModel msg)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO getintouch (name,email,subject,message) VALUES(@name,@email,@sub,@msg)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", msg.Name);
                    cmd.Parameters.AddWithValue("@email", msg.Email);
                    cmd.Parameters.AddWithValue("@sub", msg.Subject);
                    cmd.Parameters.AddWithValue("@msg", msg.Message);
                    int row = await cmd.ExecuteNonQueryAsync();
                    if (row == 1)
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
    }
}