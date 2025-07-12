namespace MakesEasy.Interfaces;
public interface ICeoInterface{
    Task<List<Dictionary<string,object>>> GetUsersByCountry();
    Task<List<Dictionary<string,object>>> GetUsersByState();
    Task<List<Dictionary<string,object>>> GetUsersByDistrict();
    Task<List<Dictionary<string,object>>> GetUsersByTaluka();
    Task<List<Dictionary<string,object>>> GetUsersByViilage();
}