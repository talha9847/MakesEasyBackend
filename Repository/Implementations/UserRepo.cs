using System.ComponentModel;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MakesEasy.Interfaces;
using MakesEasy.Models;
using Npgsql;
using Npgsql.Internal;

namespace MakesEasy.Repo;

public class UserRepo : IUserInterface
{

    private readonly string _connectionString;

    public UserRepo(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<int> UserRegister(UserModel user)
    {
        try
        {
            if (user.ConfirmPassword != user.Password)
            {
                return 0;
            }
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var querry = "SELECT 1 FROM users WHERE email=@email OR mobile=@mobile";
                using (var cmd = new NpgsqlCommand(querry, conn))
                {
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@mobile", user.Mobile);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return 2;
                        }
                    }
                }

                var query = "INSERT INTO USERS(fname,lname,email,mobile,password,country,state,dist,taluka,village,role)values(@fname,@lname,@email,@mobile,@pass,@country,@state,@dist,@taluka,@village,@role)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fname", user.FirstName);
                    cmd.Parameters.AddWithValue("@lname", user.LastName);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@mobile", user.Mobile);
                    var hashedPass = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    cmd.Parameters.AddWithValue("@pass", hashedPass);
                    cmd.Parameters.AddWithValue("@country", user.CountryId);
                    cmd.Parameters.AddWithValue("@state", user.StateId);
                    cmd.Parameters.AddWithValue("@dist", user.DistId);
                    cmd.Parameters.AddWithValue("@taluka", user.TalukaId);
                    cmd.Parameters.AddWithValue("@village", user.VillageId);
                    cmd.Parameters.AddWithValue("@role", user.Role);

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

    public async Task<UserModel> UserLogin(string identifier, string password)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,role,password FROM users WHERE (email=@identifier OR mobile=@identifier)  AND status='Approved'";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@identifier", identifier);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var hashedPass = reader.GetString(2);
                            bool verified = BCrypt.Net.BCrypt.Verify(password, hashedPass);
                            if (!verified)
                            {
                                return null;
                            }
                            return new UserModel
                            {
                                Id = reader.GetInt32(0),
                                Role = reader.GetString(1),
                                Password = null
                            };
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

    public async Task<List<Dictionary<string, object>>> PendingUsers(int villageId)
    {
        try
        {
            var Users = new List<Dictionary<string, object>>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,fname,lname,mobile,status,email FROM users WHERE role='User' AND status='Pending' AND village=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", villageId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new Dictionary<string, object>{
                                {"id",reader.GetInt32(0)},
                                {"name",reader.GetString(1)+ " "+reader.GetString(2)},
                                {"mobile",reader.GetString(3)},
                                {"status",reader.GetString(4)},
                                {"email",reader.GetString(5)}
                                // {"villageId",reader.GetInt32(5)}
                            };
                            Users.Add(user);
                        }
                    }
                }
            }

            return Users;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }


    public async Task<List<Dictionary<string, object>>> ApprovedUsers(int villageId)
    {
        try
        {
            var users = new List<Dictionary<string, object>>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,fname,lname,mobile,status,email FROM users  WHERE role = 'User' AND status = 'Approved' AND village = @id ORDER BY id;";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", villageId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new Dictionary<string, object>{
                                {"id",reader.GetInt32(0)},
                                {"name",reader.GetString(1)+" "+reader.GetString(2)},
                                {"mobile",reader.GetString(3)},
                                {"status",reader.GetString(4)},
                                {"email",reader.GetString(5)}
                            };
                            users.Add(user);

                        }
                    }
                }
            }
            return users;

        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }



    public async Task<List<Dictionary<string, object>>> RejectedUsers(int villageId)
    {
        try
        {
            var users = new List<Dictionary<string, object>>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,fname,lname,mobile,status,email FROM users  WHERE role = 'User' AND status = 'Rejected' AND village = @id ORDER BY id;";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", villageId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new Dictionary<string, object>{
                                {"id",reader.GetInt32(0)},
                                {"name",reader.GetString(1)+" "+reader.GetString(2)},
                                {"mobile",reader.GetString(3)},
                                {"status",reader.GetString(4)},
                                {"email",reader.GetString(5)}
                            };
                            users.Add(user);

                        }
                    }
                }
            }
            return users;

        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }

    public async Task<int> UpdateStatus(int id, string status, string type, int typeId)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var allowedColumns = new HashSet<string> { "village", "taluka", "dist" };
                if (!allowedColumns.Contains(type))
                {
                    return -1;
                }
                var query = $"UPDATE users SET status=@status WHERE id=@id AND {type}=@typeId ";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@typeId", typeId);
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



    public async Task<UserModel> GetOne(int id)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var query = "SELECT fname,lname,email,mobile,village,taluka,dist,state,country,role FROM users WHERE id=@id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var user = new UserModel
                            {
                                FirstName = reader.GetString(0),
                                LastName = reader.GetString(1),
                                Email = reader.GetString(2),
                                Mobile = reader.GetString(3),
                                VillageId = reader.GetInt32(4),
                                TalukaId = reader.GetInt32(5),
                                DistId = reader.GetInt32(6),
                                StateId = reader.GetInt32(7),
                                CountryId = reader.GetInt32(8),
                                Role = reader.GetString(9),

                            };
                            return user;
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

    public async Task<int> GetUserByEmail(string email)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id FROM users WHERE email=@email";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int Id = 0;

                        if (await reader.ReadAsync())
                        {
                            Id = reader.GetInt32(0);

                        }
                        if (Id != 0)
                        {
                            return Id;
                        }
                    }
                }
            }
            return 0;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return 0;
        }
    }

    public async Task<int> TokenData(int id, string token, DateTime expiry)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO TokenData(userid,token,expiration)VALUES(@id,@token,@expiration)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@expiration", expiry);

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

    public async Task<bool> IsEmailExist(string email)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT 1 FROM users WHERE email=@email";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error :" + ex.Message);
            return false;
        }
    }


    public string GenerateOTP()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[6];
        rng.GetBytes(bytes);

        var otp = new StringBuilder();
        foreach (var b in bytes)
        {
            otp.Append((b % 10).ToString());
        }

        return otp.ToString();
    }


    public string HashOTP(string otp)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(otp);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<int> SaveOtp(string email, string otp)
    {
        try
        {
            var otpHash = HashOTP(otp);
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO userotps(email,otp_hash,expires_at) VALUES(@email,@otp,@expires)";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@otp", otpHash);
                    cmd.Parameters.AddWithValue("@expires", expiresAt);
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

    public async Task<int> GetTriedCount(string email)
    {
        try
        {
            int count = 0;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) userotps WHERE email=@email AND created_at>=NOW()::date";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            count = reader.GetInt32(0);
                            return count;
                        }
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

    public async Task<bool> VerifyOtp(string email, string Otp)
    {
        try
        {
            var hashOtp = HashOTP(Otp);
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,is_used,expires_at FROM userotps WHERE email=@email AND otp_hash=@hash ORDER BY created_at DESC LIMIT 1";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@hash", hashOtp);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int Id = reader.GetInt32(0);
                            bool isUsed = reader.GetBoolean(1);
                            DateTime expiresAt = reader.GetDateTime(2);

                            if (isUsed || DateTime.UtcNow > expiresAt)
                            {
                                return false;
                            }

                            await reader.CloseAsync();

                            var query1 = "UPDATE userotps SET is_used=TRUE WHERE id=@id";
                            using (var cm = new NpgsqlCommand(query1, conn))
                            {
                                cm.Parameters.AddWithValue("@id", Id);
                                int row = await cm.ExecuteNonQueryAsync();
                                if (row == 1)
                                {
                                    return true;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }


#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    public async Task<TokenModel> GetToken(string token)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
    {
        
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT userid,expiration,used,id FROM tokendata WHERE token=@token   AND expiration > NOW()";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new TokenModel
                            {
                                UserId = reader.GetInt32(0),
                                expiry = reader.GetDateTime(1),
                                Used = reader.GetBoolean(2),
                                Id = reader.GetInt32(3)
                            };
                       
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


    public async Task<int> UpdatePassword(int id, string password, int tokenId)
    {
        try
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(password);
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var transaction = await conn.BeginTransactionAsync())
                {
                    var query = "UPDATE users SET password=@password WHERE id=@id";
                    using (var cmd = new NpgsqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@password", hashed);
                        cmd.Parameters.AddWithValue("@id", id);
                        int row = await cmd.ExecuteNonQueryAsync();
                        if (row != 1)
                        {
                            await transaction.RollbackAsync();
                            return 0;
                        }
                    }

                    var query2 = "UPDATE tokendata SET used=TRUE WHERE id=@id ";
                    using (var cmd = new NpgsqlCommand(query2, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", tokenId);
                        int row = await cmd.ExecuteNonQueryAsync();
                        if (row != 1)
                        {
                            await transaction.RollbackAsync();
                            return 0;
                        }
                    }

                    await transaction.CommitAsync();
                    return 1;
                }

            }
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return -1;
        }
    }


    public async Task<bool> CheckWithOldPassword(int id, string password)
    {
        try
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT password FROM users WHERE id=@id ";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string storedHash = reader.GetString(0);
                            return BCrypt.Net.BCrypt.Verify(password, storedHash);
                        }
                    }
                }
            }
            return false;
        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }

}