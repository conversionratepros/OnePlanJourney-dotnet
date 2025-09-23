namespace OnePlanPetJourney.Models;

public class PetListViewModel
{
    public int LeadId { get; set; }

    public List<Pet> Pets { get; set; } = new();
}