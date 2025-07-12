namespace MakesEasy.Models;

public class FourMonthModel
{
    public int Id { get; set; }
    public int PId{ get; set; }
    public string? Name { get; set; }
    public int Total { get; set; }
    public string Place{ get; set; }
    public DateOnly LastTime { get; set; }
}