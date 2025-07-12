using System.Security.Cryptography.X509Certificates;
using MakesEasy.Interfaces;
using MakesEasy.Models;
using Npgsql;

namespace MakesEasy.Repo;

public class PeopleRepo : IPeopleInterface
{
    private readonly string _connectionString;
    public PeopleRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertPeople(PeopleModel people, int vId, int tId, int dId, int sId, int cId)
    {
        try
        {

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO people (name,mobile,age,waqt,village_id,taluka_id,dist_id,state_id,country_id,occupation) VALUES(@name,@mobile,@age,@waqt,@village,@taluka,@dist,@state,@country,@occupation)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", people.Name);
                    cmd.Parameters.AddWithValue("@mobile", people.Mobile);
                    cmd.Parameters.AddWithValue("@age", people.Age);
                    cmd.Parameters.AddWithValue("@waqt", people.Waqt);
                    cmd.Parameters.AddWithValue("@village", vId);
                    cmd.Parameters.AddWithValue("@taluka", tId);
                    cmd.Parameters.AddWithValue("@dist", dId);
                    cmd.Parameters.AddWithValue("@state", sId);
                    cmd.Parameters.AddWithValue("@country", cId);
                    cmd.Parameters.AddWithValue("@occupation", people.OccupationId);

                    int row = await cmd.ExecuteNonQueryAsync();
                    if (row == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
    }


    public async Task<List<PeopleModel>> GetPeopleByVillage(string role, int villageId)
    {
        try
        {
            var peoples = new List<PeopleModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = $"SELECT p.id,p.name,p.mobile,p.age,p.waqt,o.occupation,o.id FROM people p JOIN occupations o ON p.occupation=o.id WHERE p.{role}={villageId} ";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", villageId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var people = new PeopleModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Mobile = reader.GetString(2),
                                Age = reader.GetInt32(3),
                                Waqt = reader.GetString(4),
                                Occupation = reader.GetString(5),
                                OccupationId = reader.GetInt32(6)
                            };
                            peoples.Add(people);
                        }
                    }
                }
            }
            return peoples;
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }


    public async Task<int> UpdatePeople(PeopleModel people)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "UPDATE people SET name=@name,mobile=@mobile,age=@age,waqt=@waqt, occupation=@occ WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", people.Id);
                    cmd.Parameters.AddWithValue("@name", people.Name);
                    cmd.Parameters.AddWithValue("@mobile", people.Mobile);
                    cmd.Parameters.AddWithValue("@age", people.Age);
                    cmd.Parameters.AddWithValue("@waqt", people.Waqt);
                    cmd.Parameters.AddWithValue("@occ", people.OccupationId);
                    int row = await cmd.ExecuteNonQueryAsync();
                    if (row == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
    }


    public async Task<int> DeletePeople(int id)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "DELETE FROM people WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int row = await cmd.ExecuteNonQueryAsync();
                    if (row == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
    }


    public async Task<Dictionary<string, object>> GetCount(string role, int villageId)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string columnName = role switch
                {
                    "village_id" => "village_id",
                    "dist_id" => "dist_id",
                    "taluka_id" => "taluka_id",
                    _ => null
                };
                if (string.IsNullOrEmpty(columnName))
                {
                    return null;
                }

                var query = $@"SELECT
                            COUNT(id) AS Members,
                            COUNT(CASE WHEN occupation = 1 THEN 1 END) AS Students,
                            COUNT(CASE WHEN waqt = '4 Month' THEN 1 END) AS Months,
                            COUNT(CASE WHEN waqt = '40 Days' THEN 1 END) AS Days
                        FROM people
                        WHERE {role}=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", villageId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var counts = new Dictionary<string, object>{
                                {"members",reader.GetInt32(0)},
                                {"students",reader.GetInt32(1)},
                                {"40Days",reader.GetInt32(2)},
                                {"4Months",reader.GetInt32(3)}
                            };
                            return counts;

                        }
                    }
                }
            }
            return null;

        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }

    public async Task<int> InsertStudent(StudentModel student)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO students(name,mobile,age,waqt,field,year,village_id,taluka_id,dist_id,state_id,country_id) VALUES(@name,@mobile,@age,@waqt,@field,@year,@village,@taluka,@dist,@state,@country)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@mobile", student.Mobile);
                    cmd.Parameters.AddWithValue("@age", student.Age);
                    cmd.Parameters.AddWithValue("@waqt", student.Waqt);
                    cmd.Parameters.AddWithValue("@field", student.Field);
                    cmd.Parameters.AddWithValue("@year", student.Year);
                    cmd.Parameters.AddWithValue("@village", student.VillageId);
                    cmd.Parameters.AddWithValue("@taluka", student.TalukaId);
                    cmd.Parameters.AddWithValue("@dist", student.DistId);
                    cmd.Parameters.AddWithValue("@state", student.StateId);
                    cmd.Parameters.AddWithValue("@country", student.CountryId);

                    int row = cmd.ExecuteNonQuery();
                    if (row == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Errorr: " + ex.Message);
            return -1;
        }
    }


    public async Task<List<StudentModel>> GetStudents(int id, string role)
    {
        try
        {
            var students = new List<StudentModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var allowedColumns = new HashSet<string> { "village_id", "taluka_id", "dist_id" };
                if (!allowedColumns.Contains(role))
                {
                    throw new ArgumentException("Invalid");
                }
                var query = $"SELECT id,name,mobile,age,waqt,field,year FROM students WHERE {role} = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var student = new StudentModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Mobile = reader.GetString(2),
                                Age = reader.GetInt32(3),
                                Waqt = reader.GetString(4),
                                Field = reader.GetString(5),
                                Year = reader.GetString(6)
                            };
                            students.Add(student);

                        }
                    }
                }
            }
            return students;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error this : " + ex.Message);
            return null;
        }
    }

    public async Task<int> UpdateStudent(StudentModel student)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "UPDATE students SET name=@name, mobile=@mobile, age=@age, waqt=@waqt, field=@field, year=@year WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", student.Id);
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@mobile", student.Mobile);
                    cmd.Parameters.AddWithValue("@age", student.Age);
                    cmd.Parameters.AddWithValue("@waqt", student.Waqt);
                    cmd.Parameters.AddWithValue("@field", student.Field);
                    cmd.Parameters.AddWithValue("@year", student.Year);

                    int row = cmd.ExecuteNonQuery();

                    if (row == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine(ex.Message);
            return -1;
        }
    }

    public async Task<int> DeleteStudent(int id)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "DELETE FROM students WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id",id);
                    int row = cmd.ExecuteNonQuery();
                    if (row == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
    }



}