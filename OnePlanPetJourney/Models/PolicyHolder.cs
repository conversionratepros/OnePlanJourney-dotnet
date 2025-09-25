using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Models;

public class PolicyHolder
{
    [Key]
    public int Id { get; set; }


    [Required(ErrorMessage = "ID number required")]
    public string IdNumber { get; set; }

    [Required(ErrorMessage = "First name required")]
    public string FirstName { get; set; }


    [Required(ErrorMessage = "Last name required")]
    public string LastName { get; set; }


    [Required(ErrorMessage = "Title required")]
    public string Title { get; set; }


    [Required(ErrorMessage = "Phone number required")]
    public string MobileNumber { get; set; }


    [Required(ErrorMessage = "Alternative phone number required")]
    public string AltMobileNumber { get; set; }


    [Required(ErrorMessage = "Email required")]
    public string EmailAddress { get; set; }


    [Required(ErrorMessage = "Gender required")]
    public string Gender { get; set; }


    public bool hasConcented { get; set; }
     
     public bool marketing { get; set; }
    

    
    



   

}
