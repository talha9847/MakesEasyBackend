using MakesEasy.Models;

namespace MakesEasy.Interfaces;
public interface ILocationInterface{
    Task<List<CountryModel>> GetCountries();
    Task<List<StateModel>> GetStates(int countryId);
    Task<List<DistModel>> GetDistricts(int stateId);
    Task<List<TalukaModel>> GetTalukas(int districtId);
    Task<List<VillageModel>> GetVillages(int talukaId);
    Task<List<OccupationModel>> GetOccupations();

}