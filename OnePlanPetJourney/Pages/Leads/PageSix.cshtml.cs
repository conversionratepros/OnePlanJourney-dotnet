using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageSixModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageSixModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public PolicyHolder Input { get; set; } = new();

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            BuildTitleOptions();
            BuildGenderOptions();

            // DbSet is usually plural: PolicyHolders
            var entity = await _db.PolicyHolder.FirstOrDefaultAsync(p => p.Id == id);
            Input = entity ?? new PolicyHolder { Id = 0 };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BuildTitleOptions();
            BuildGenderOptions();

            if (!ModelState.IsValid) return Page();

            PolicyHolder? entity = null;

            if (Input.Id > 0)
            {
                entity = await _db.PolicyHolder.FirstOrDefaultAsync(p => p.Id == Input.Id);
            }

            if (entity is null)
            {
                entity = new PolicyHolder();
                _db.PolicyHolder.Add(entity);
            }

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
            return RedirectToPage("/Leads/PageSeven", new { id = entity.Id });
        }
    }
}
