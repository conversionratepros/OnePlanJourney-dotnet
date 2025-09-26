using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnePlanPetJourney.Models;
// using YourDbContextNamespace;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageSevenModel : PageModel
    {
        // private readonly YourDbContext _db;
        // public PageSevenModel(YourDbContext db) { _db = db; }

        [BindProperty]
        public Address AddressInput { get; set; } = new Address();

        [BindProperty]
        public string? Mode { get; set; } // "old" or "tech"

        public int LeadId { get; set; }

        public IActionResult OnGet(int id)
        {
            LeadId = id;
            AddressInput.leadId = id;
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            // Ensure lead id sticks even if someone tampers with the form
            AddressInput.leadId = id;
            LeadId = id;

            // You can branch on Mode here if you want custom validation
            // if (Mode == "old") { /* only physical must be filled */ }
            // if (Mode == "tech") { /* physical + delivery */ }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // _db.Addresses.Add(AddressInput);
            // _db.SaveChanges();

            TempData["Message"] = "Address saved.";
            // next page:
            return RedirectToPage("/Leads/PageEight", new { id });
        }
    }
}
