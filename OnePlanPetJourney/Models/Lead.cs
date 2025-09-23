using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Models;

public class Lead
{
    [Key]                           
    public int Id { get; set; }     

    
    public int CatCount { get; set; }
    public int DogCount { get; set; }

    public string? MobileNumber {get; set;}

    [Required, MaxLength(100)]
    public string? FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string? LastName { get; set; } = string.Empty;


    [DataType(DataType.Date)]
    public DateTime? DOB { get; set; } = DateTime.UtcNow;

    public string? Province {get; set;}

    public string? Email { get; set;}

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();

}
