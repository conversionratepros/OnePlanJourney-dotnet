using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageFiveModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageFiveModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public int LeadId { get; set; }
            public List<Pet> Pets { get; set; } = new();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Verify lead exists
            var leadExists = await _db.Leads.AnyAsync(l => l.Id == id);
            if (!leadExists)
            {
                TempData["Message"] = "Lead not found.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            // Load pets for this lead
            var pets = await _db.Pets
                .Where(p => p.LeadId == id)
                .OrderBy(p => p.Id)
                .ToListAsync();

            if (pets.Count == 0)
            {
                TempData["Message"] = "No pets found for this lead.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            Input = new InputModel
            {
                LeadId = id,
                Pets = pets
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // No persistence needed yet; just proceed
            TempData["Message"] = "Exclusions reviewed.";
            return RedirectToPage("/Leads/PageSix", new { id = Input.LeadId });
        }
    }
}
