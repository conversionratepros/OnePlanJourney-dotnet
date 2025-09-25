using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageOneModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public PageOneModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
            public int CatCount { get; set; }

            [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
            public int DogCount { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // 1) Create the Lead
            var lead = new Lead
            {
                CatCount = Input.CatCount,
                DogCount = Input.DogCount,
                FirstName = "Unknown",
                LastName = "Unknown",
                DOB = DateTime.Today
            };

            _db.Leads.Add(lead);
            await _db.SaveChangesAsync(); // sets lead.Id

            // 2) Add Pet placeholders
            DateTime defaultDob = DateTime.Today;
            const string unknown = "Unknown";
            const string none = "None";

            for (int i = 0; i < lead.CatCount; i++)
            {
                _db.Pets.Add(new Pet
                {
                    LeadId = lead.Id,
                    Name = $"Cat {i + 1}",
                    DOB = defaultDob,
                    IsMale = false,
                    IsDog = false,
                    Breed = unknown,
                    BreedSize = unknown,
                    Colour = unknown,
                    ChipNumber = string.Empty,
                    PreferredVet = unknown,
                    IsNeutered = false,
                    Injuries = none,
                    MedicalCondition = none,
                    AddOns = string.Empty,
                });
            }

            for (int i = 0; i < lead.DogCount; i++)
            {
                _db.Pets.Add(new Pet
                {
                    LeadId = lead.Id,
                    Name = $"Dog {i + 1}",
                    DOB = defaultDob,
                    IsMale = false,
                    IsDog = true,
                    Breed = unknown,
                    BreedSize = unknown,
                    Colour = unknown,
                    ChipNumber = string.Empty,
                    PreferredVet = unknown,
                    IsNeutered = false,
                    Injuries = none,
                    MedicalCondition = none,
                    AddOns = string.Empty,
                });
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = $"Lead {lead.Id} created with {lead.CatCount} cat(s) and {lead.DogCount} dog(s).";

            // Redirect to the next Razor Page (create it at /Pages/Leads/PageTwo.cshtml)
            
            return RedirectToPage("PageTwo", new { id = lead.Id });
            



        }
    }
}
