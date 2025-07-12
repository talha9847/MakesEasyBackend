using MakesEasy.Models;

namespace MakesEasy.Interfaces;

public interface IUserInterface
{
    Task<int> UserRegister(UserModel user);
    Task<UserModel> UserLogin(string identifier, string password);
    Task<List<Dictionary<string, object>>> PendingUsers(int villageId);
    Task<List<Dictionary<string, object>>> ApprovedUsers(int villageId);
    Task<List<Dictionary<string, object>>> RejectedUsers(int villageId);
    Task<int> UpdateStatus(int id, string status, string type, int typeId);
    Task<UserModel> GetOne(int id);
    Task<int> GetUserByEmail(string email);
    Task<int> TokenData(int id, string token, DateTime expiry);
    Task<bool> IsEmailExist(string email);
    Task<int> SaveOtp(string email, string otp);
    string GenerateOTP();
    string HashOTP(string otp);
    Task<int> GetTriedCount(string email);
    Task<bool> VerifyOtp(string email, string Otp);
    Task<TokenModel> GetToken(string token);
    Task<int> UpdatePassword(int id, string password,int tokenId);

    Task<bool> CheckWithOldPassword(int id,string password);
}