using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Models;

public class Plan
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "No Plan Set")]
    public string PlanName { get; set; }


    [Required(ErrorMessage = "No Plan Set")]
    public int PlanPrice { get; set; }

 
    public bool IsActive { get; set; }
     
    
}
