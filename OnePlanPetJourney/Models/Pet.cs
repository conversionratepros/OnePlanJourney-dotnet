using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Models;

public class Pet
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "No Lead ID set.")]
    public int LeadId { get; set; }
     
    [Required(ErrorMessage = "Please add a pet First Name")]
    public string Name { get; set; } = string.Empty;
     
    [Required(ErrorMessage = "Please add a date of birth")]
    [DataType(DataType.Date)]
    public DateTime DOB { get; set; }

    [Required(ErrorMessage = "Please select male of female")]
    public bool IsMale { get; set; }

    [Required(ErrorMessage = "Please select cat or dog")]
    public bool IsDog { get; set; }

    [Required(ErrorMessage = "Please add a breed")]
    public string Breed { get; set; } 

     public string BreedSize { get; set; } 


    [Required(ErrorMessage = "Please add a colour")]
    public string Colour { get; set; } 


    public string ChipNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a vet")]
    public string PreferredVet { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select if neutered or not")]
     public bool IsNeutered { get; set; } 

    [Required(ErrorMessage = "Please select a injury option")]
    public string Injuries { get; set; } = string.Empty;


    [Required(ErrorMessage = "Please select a medical condition option ")]
    public string MedicalCondition { get; set; } = string.Empty;



    public int? PlanType { get; set; }
}
