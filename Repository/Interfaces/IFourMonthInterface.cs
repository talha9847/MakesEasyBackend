using MakesEasy.Models;
namespace MakesEasy.Interfaces;

public interface IFourMonthInterface
{
    Task<List<FourMonthModel>> Get4Companions(string role, int roleId);
    Task<int> AddCompanionsData(FourMonthModel model);
    Task<int> AddCompanionData(FourMonthModel model);
    Task<int> UpdateCompanionsData(FourMonthModel model);
    Task<int> UpdateCompanionData(FourMonthModel model);
    Task<List<FourMonthModel>> GetData(int id);
    Task<List<FourMonthModel>> Get40Companion(string role, int roleId);
    Task<List<FourMonthModel>> Get40Data(int id);
}