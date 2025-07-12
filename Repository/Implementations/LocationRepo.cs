using MakesEasy.Interfaces;
using MakesEasy.Models;
using Npgsql;

namespace MakesEasy.Repo;
public class LocationRepo : ILocationInterface
{
    private readonly string _connectionString;
    public LocationRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<CountryModel>> GetCountries()
    {
        try
        {
            var countries = new List<CountryModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,name FROM countries ORDER BY name";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var coun = new CountryModel
                            {
                                CountryId = reader.GetInt32(0),
                                CountryName = reader.GetString(1),

                            };
                            countries.Add(coun);
                        }
                    }
                }
            }
            return countries;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error:" + ex.Message);
            return null;
        }
    }


    public async Task<List<StateModel>> GetStates(int countryId)
    {
        try
        {
            var states = new List<StateModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,name FROM states WHERE country_id=@id ORDER BY name";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", countryId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var state = new StateModel
                            {
                                StateId = reader.GetInt32(0),
                                StateName = reader.GetString(1),

                            };
                            states.Add(state);
                        }
                    }
                }
            }
            return states;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error:" + ex.Message);
            return null;
        }
    }




    public async Task<List<DistModel>> GetDistricts(int stateId)
    {
        try
        {
            var districts = new List<DistModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,name FROM districts WHERE state_id=@id ORDER by name";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", stateId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dist = new DistModel
                            {
                                DistId = reader.GetInt32(0),
                                DistName = reader.GetString(1),

                            };
                            districts.Add(dist);
                        }
                    }
                }
            }
            return districts;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error:" + ex.Message);
            return null;
        }
    }


    public async Task<List<TalukaModel>> GetTalukas(int districtId)
    {
        try
        {
            var talukas = new List<TalukaModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,name FROM talukas WHERE district_id=@id ORDER BY name";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", districtId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var taluka = new TalukaModel
                            {
                                TalukaId = reader.GetInt32(0),
                                TalukaName = reader.GetString(1),

                            };
                            talukas.Add(taluka);
                        }
                    }
                }
            }
            return talukas;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error:" + ex.Message);
            return null;
        }
    }


    public async Task<List<VillageModel>> GetVillages(int talukaId)
    {
        try
        {
            var villages = new List<VillageModel>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT id,name FROM villages WHERE taluka_id=@id ORDER BY name";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", talukaId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var village = new VillageModel
                            {
                                VillageId = reader.GetInt32(0),
                                VillageName = reader.GetString(1),

                            };
                            villages.Add(village);
                        }
                    }
                }
            }
            return villages;

        }
        catch (System.Exception ex)
        {

            System.Console.WriteLine("Error:" + ex.Message);
            return null;
        }
    }

    public async Task<List<OccupationModel>> GetOccupations(){
        try
        {
            var occupations=new List<OccupationModel>();
            using(var conn=new NpgsqlConnection(_connectionString)){
                await conn.OpenAsync();

                var query="SELECT id,occupation FROM occupations";
                using(var cmd=new NpgsqlCommand(query,conn)){
                    using(var reader=await cmd.ExecuteReaderAsync()){
                        while(await reader.ReadAsync()){
                            var occ=new OccupationModel{
                                Id=reader.GetInt32(0),
                                Occupation=reader.GetString(1)
                            };
                            occupations.Add(occ);
                        }
                    }
                }
            }
            return occupations;
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("Error: "+ex.Message);
            return null;
        }
    }
}