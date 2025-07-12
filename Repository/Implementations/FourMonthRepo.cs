using MakesEasy.Interfaces;
using MakesEasy.Models;
using Npgsql;

namespace MakesEasy.Repo;

public class FourMonthRepo : IFourMonthInterface
{
    private readonly string _connectionString;

    public FourMonthRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<FourMonthModel>> Get4Companions(string role, int roleId)
    {
        try
        {
            var columnMap = new Dictionary<string, string>{
                    {"village_id","village_id"},
                    {"taluka_id","taluka_id"},
                    {"dist_id","dist_id"}
            };
            if (!columnMap.ContainsKey(role))
            {
                return null;
            }

            string columnName = columnMap[role];
            var CompanionList = new List<FourMonthModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = $"SELECT p.id,p.name,GREATEST(COUNT(d.id),1) AS Total, COALESCE(MAX(d.date),DATE '1111-11-11') As LastTime FROM people p  LEFT JOIN fourmonthsdata d ON p.id=d.peopleid WHERE p.waqt='4 Month' AND p.{columnName}=@id GROUP BY p.id ORDER BY COUNT(d.id) DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", roleId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var comp = new FourMonthModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Total = reader.GetInt32(2),
                                LastTime = DateOnly.FromDateTime(reader.GetDateTime(3))
                            };
                            CompanionList.Add(comp);
                        }
                    }
                }
            }
            return CompanionList;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }


    public async Task<int> AddCompanionsData(FourMonthModel model)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO fourmonthsdata(peopleid,date,places) VALUES(@pid,@date,@place)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pid", model.PId);
                    cmd.Parameters.AddWithValue("@date", model.LastTime);
                    cmd.Parameters.AddWithValue("@place", model.Place);
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


    public async Task<List<FourMonthModel>> GetData(int id)
    {
        try
        {
            var data = new List<FourMonthModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,date,places FROM fourmonthsdata WHERE peopleid=@id ORDER BY date DESC";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var d = new FourMonthModel
                            {
                                Id = reader.GetInt32(0),
                                LastTime = DateOnly.FromDateTime(reader.GetDateTime(1)),
                                Place = reader.GetString(2)
                            };
                            data.Add(d);
                        }
                    }
                }
            }
            return data;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }
    public async Task<List<FourMonthModel>> Get40Companion(string role, int roleId)
    {
        try
        {
            Dictionary<string, string> columns = new Dictionary<string, string>
                {
                    {"village_id","village_id"},
                    {"taluka_id","taluka_id"},
                    {"dist_id","dist_id"}
                };

            if (!columns.ContainsKey(role))
            {
                return null;
            }

            string columnName = columns[role];
            var datas = new List<FourMonthModel>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var query = $"SELECT p.id,p.name,GREATEST(COUNT(d.id),1) AS Total, COALESCE(MAX(d.date),DATE '1111-11-11') As LastTime FROM people p  LEFT JOIN fourtydaysdata d ON p.id=d.peopleid WHERE p.waqt='4 Month' AND p.{columnName}=@id GROUP BY p.id ORDER BY COUNT(d.id) DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", roleId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = new FourMonthModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Total = reader.GetInt32(2),
                                LastTime = DateOnly.FromDateTime(reader.GetDateTime(3))
                            };
                            datas.Add(data);
                        }
                    }
                }
            }
            return datas;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }

    public async Task<List<FourMonthModel>> Get40Data(int id)
    {
        try
        {
            var datas = new List<FourMonthModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,date,places FROM fourtydaysdata WHERE peopleid=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = new FourMonthModel
                            {
                                Id = reader.GetInt32(0),
                                LastTime = DateOnly.FromDateTime(reader.GetDateTime(1)),
                                Place = reader.GetString(2)
                            };
                            datas.Add(data);
                        }
                    }
                }
            }
            return datas;
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }


    public async Task<int> UpdateCompanionsData(FourMonthModel model)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "UPDATE fourmonthsdata SET places = @place, date =@date where id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", model.Id);
                    cmd.Parameters.AddWithValue("@date", model.LastTime);
                    cmd.Parameters.AddWithValue("@place", model.Place);
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

    public async Task<int> AddCompanionData(FourMonthModel model)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO fourtydaysdata(peopleid,date,places) VALUES(@pId,@date,@place)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pId", model.PId);
                    cmd.Parameters.AddWithValue("@date", model.LastTime);
                    cmd.Parameters.AddWithValue("@place", model.Place);
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

   public async  Task<int> UpdateCompanionData(FourMonthModel model)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "UPDATE fourtydaysdata SET date=@date,places=@place WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@date", model.LastTime);
                    cmd.Parameters.AddWithValue("@place", model.Place);
                    cmd.Parameters.AddWithValue("@id", model.Id);
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