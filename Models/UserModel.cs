using System.ComponentModel.DataAnnotations;

namespace MakesEasy.Models;

public class UserModel
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
    [Required]
    public string Mobile { get; set; }
    public string? Role { get; set; }
    [Required]
    public int CountryId { get; set; }
    [Required]
    public int DistId { get; set; }
    [Required]
    public int StateId { get; set; }
    [Required]
    public int TalukaId { get; set; }
    [Required]
    public int VillageId { get; set; }

}