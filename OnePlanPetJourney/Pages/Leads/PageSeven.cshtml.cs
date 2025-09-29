using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageSevenModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageSevenModel(ApplicationDbContext db) => _db = db;

        // Bind directly to your PolicyHolder entity
        [BindProperty]
        public PolicyHolder Input { get; set; } = new();

        // Dropdowns
        public List<SelectListItem> TitleOptions { get; private set; } = new();
        public List<SelectListItem> GenderOptions { get; private set; } = new();

        private void BuildTitleOptions()
        {
            TitleOptions = new()
            {
                new("Mr", "Mr"),
                new("Mrs", "Mrs"),
                new("Ms", "Ms"),
                new("Dr", "Dr"),
                new("Prof", "Prof")
            };
        }

        private void BuildGenderOptions()
        {
            GenderOptions = new()
            {
                new("Male", "Male"),
                new("Female", "Female"),
                new("Other", "Other"),
                new("Prefer not to say", "Prefer not to say")
            };
        }

        // GET /Leads/PageSeven/{id}  -> id = LeadId
        public async Task<IActionResult> OnGetAsync(int id)
        {
            BuildTitleOptions();
            BuildGenderOptions();

            // Try load by LeadId (one PolicyHolder per lead)
            var entity = await _db.PolicyHolder.FirstOrDefaultAsync(p => p.LeadId == id);

            if (entity is null)
            {
                // brand new record for this lead
                Input = new PolicyHolder { LeadId = id };
            }
            else
            {
                Input = entity;
            }

            return Page();
        }

        // POST
        public async Task<IActionResult> OnPostAsync()
        {
            BuildTitleOptions();
            BuildGenderOptions();

            if (!ModelState.IsValid)
                return Page();

            // Find existing PolicyHolder for this lead (or create)
            var entity = await _db.PolicyHolder.FirstOrDefaultAsync(p => p.LeadId == Input.LeadId);
            if (entity is null)
            {
                entity = new PolicyHolder { LeadId = Input.LeadId };
                _db.PolicyHolder.Add(entity);
            }

            // Map fields
            entity.IdNumber        = Input.IdNumber;
            entity.FirstName       = Input.FirstName;
            entity.LastName        = Input.LastName;
            entity.Title           = Input.Title;
            entity.MobileNumber    = Input.MobileNumber;
            entity.AltMobileNumber = Input.AltMobileNumber;
            entity.EmailAddress    = Input.EmailAddress;
            entity.Gender          = Input.Gender;
            entity.hasConcented    = Input.hasConcented;
            entity.marketing       = Input.marketing;

            await _db.SaveChangesAsync();

            TempData["Message"] = "Policy holder details saved.";

            // Proceed to Page Eight using LeadId
            return RedirectToPage("/Leads/PageEight", new { id = entity.LeadId });
        }
    }
}
