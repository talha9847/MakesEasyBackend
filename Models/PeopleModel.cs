using System.Text.Json.Serialization;

namespace MakesEasy.Models;
public class PeopleModel{
   public int Id{get;set;}
   public string Name{get;set;}
    public string Mobile{get;set;}
    public int Age{get;set;}
    public string Waqt{get;set;}
    public int OccupationId {get;set;}
    public string? Occupation{get;set;}
}