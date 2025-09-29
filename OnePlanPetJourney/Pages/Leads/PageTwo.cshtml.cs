using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageTwoModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public PageTwoModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public int Id { get; set; }

            [MaxLength(100)]
            public string? FirstName { get; set; } = string.Empty;

            [MaxLength(100)]
            public string? LastName { get; set; } = string.Empty;

             [MaxLength(100)]
            public string? MobileNumber { get; set; } = string.Empty;

            [DataType(DataType.Date)]
            public DateTime? DOB { get; set; }

            public string? Province { get; set; }

            [EmailAddress]
            public string? Email { get; set; }
        }

        // GET /Leads/PageTwo/{id}
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var lead = await _db.Leads.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (lead == null) return NotFound();

            Input = new InputModel
            {
                Id        = lead.Id,
                FirstName = lead.FirstName,
                LastName  = lead.LastName,
                DOB       = lead.DOB,
                Province  = lead.Province,
                Email     = lead.Email
            };

            return Page();
        }

        // POST back to same page
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var lead = await _db.Leads.FirstOrDefaultAsync(l => l.Id == Input.Id);
            if (lead == null) return NotFound();

            // update only the intended fields
            lead.FirstName = Input.FirstName;
            lead.LastName  = Input.LastName;
            lead.DOB       = Input.DOB;
            lead.Province  = Input.Province;
            lead.Email     = Input.Email;

            await _db.SaveChangesAsync();

            //TempData["Message"] = "Lead details updated successfully.";

            // keep using your MVC step for now:
            return RedirectToPage("PageThree", new { id = lead.Id });



            // when you migrate that step to Razor Pages, switch to:
            // return RedirectToPage("/Pets/AddPetDetails", new { leadId = lead.Id, area = "" });
        }
    }
}
