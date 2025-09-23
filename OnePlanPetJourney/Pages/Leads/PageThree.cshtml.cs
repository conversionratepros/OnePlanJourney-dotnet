using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; 

//This is page three
namespace OnePlanPetJourney.Pages.Leads
{
    public class PageThreeModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageThreeModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public int LeadId { get; set; }
            public List<Pet> Pets { get; set; } = new();
        }

        public List<SelectListItem> VetOptions { get; set; } = new();

            private void BuildVetOptions()
            {
                VetOptions = new List<SelectListItem>
                {
                    new("— Select a vet —", ""),
                    new("No preferred vet", "None"),
                    new("Banhoek Animal Clinic", "Banhoek Animal Clinic"),
                    new("Sea Point Veterinary Hospital", "Sea Point Veterinary Hospital"),
                    new("Panorama Veterinary Clinic", "Panorama Veterinary Clinic"),
                    new("Other", "Other") // optional, if you want an "Other" path
                };
            }

        public List<SelectListItem> InjuryOptions { get; set; } = new();
        private void BuildInjuryOptions()
        {
            InjuryOptions = new List<SelectListItem>
                {
                    new("— Select injury status —", ""),
                    new("None", "None"),
                    new("Fracture", "Fracture"),
                    new("Allergic Reaction", "Allergic Reaction"),
                    new("Surgery Recovery", "Surgery Recovery"),
                    new("Skin Condition", "Skin Condition"),
                    new("Other", "Other")
                };
        }

        public List<SelectListItem> MedicalConditionOptions { get; set; } = new();

            private void BuildMedicalConditionOptions()
            {
                MedicalConditionOptions = new List<SelectListItem>
                {
                    new("— Select medical condition —", ""),
                    new("None", "None"),
                    new("Diabetes", "Diabetes"),
                    new("Heart Disease", "Heart Disease"),
                    new("Arthritis", "Arthritis"),
                    new("Allergies", "Allergies"),
                    new("Other", "Other")
                };
            }




        public async Task<IActionResult> OnGetAsync(int id)
        {
            var leadExists = await _db.Leads.AnyAsync(l => l.Id == id);
            if (!leadExists)
            {
                TempData["Message"] = "Owner not found.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            var pets = await _db.Pets.Where(p => p.LeadId == id)
                                     .OrderBy(p => p.Id)
                                     .ToListAsync();

            if (pets.Count == 0)
            {
                TempData["Message"] = "No pets found for this lead.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }
            BuildVetOptions();
            BuildInjuryOptions();
            BuildMedicalConditionOptions();


            Input = new InputModel { LeadId = id, Pets = pets };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await _db.Leads.AnyAsync(l => l.Id == Input.LeadId))
                ModelState.AddModelError(string.Empty, "Owner not found.");

            if (!ModelState.IsValid) return Page();

             BuildVetOptions();
             BuildInjuryOptions();
            BuildMedicalConditionOptions();



            foreach (var pet in Input.Pets)
            {
                var existing = await _db.Pets
                    .FirstOrDefaultAsync(p => p.Id == pet.Id && p.LeadId == Input.LeadId);
                if (existing is null) continue;

                existing.Name = pet.Name;
                existing.DOB = pet.DOB;
                existing.IsMale = pet.IsMale;
                existing.IsDog = pet.IsDog;
                existing.Breed = pet.Breed;
                existing.Colour = pet.Colour;
                existing.ChipNumber = pet.ChipNumber;
                existing.PreferredVet = pet.PreferredVet;
                existing.IsNeutered = pet.IsNeutered;
                existing.Injuries = pet.Injuries;
                existing.MedicalCondition = pet.MedicalCondition;
                existing.BreedSize = pet.BreedSize;
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Pet details saved successfully.";
            return RedirectToPage("/Leads/PageFour", new { id = Input.LeadId });
        }
    }
}
