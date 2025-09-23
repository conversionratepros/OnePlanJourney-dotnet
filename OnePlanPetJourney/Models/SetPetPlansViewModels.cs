using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OnePlanPetJourney.Models
{
    public class PetPlanItem
    {
        public int PetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDog { get; set; }

        // Selected Plan Id for this pet (nullable if not chosen yet)
        public int? PlanType { get; set; }
    }

    public class SetPetPlansViewModel
    {
        public int LeadId { get; set; }

        // One row per pet
        public List<PetPlanItem> Items { get; set; } = new();

        // Options for the dropdown: plan Id + plan name
        public List<SelectListItem> Plans { get; set; } = new();
    }
}
