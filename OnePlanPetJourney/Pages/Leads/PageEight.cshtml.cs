using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageEightModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PageEightModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BankingDetails BankingDetails { get; set; }

        public List<SelectListItem> BankNames { get; set; }
        public List<SelectListItem> AccountTypes { get; set; }
        public List<SelectListItem> HearAboutUsOptions { get; set; }

        public void OnGet()
        {
            BankingDetails = new BankingDetails();

            BankNames = new List<SelectListItem>
            {
                new SelectListItem { Value = "FNB", Text = "FNB" },
                new SelectListItem { Value = "Absa", Text = "Absa" },
                new SelectListItem { Value = "Standard Bank", Text = "Standard Bank" },
                new SelectListItem { Value = "Nedbank", Text = "Nedbank" },
                new SelectListItem { Value = "Capitec", Text = "Capitec" },
            };

            AccountTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Cheque", Text = "Cheque" },
                new SelectListItem { Value = "Savings", Text = "Savings" },
                new SelectListItem { Value = "Transmission", Text = "Transmission" },
            };

            HearAboutUsOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Friend", Text = "Friend" },
                new SelectListItem { Value = "Social Media", Text = "Social Media" },
                new SelectListItem { Value = "Google", Text = "Google" },
                new SelectListItem { Value = "Other", Text = "Other" },
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BankingDetails.Add(BankingDetails);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Leads/PageNine"); // Or a confirmation page
        }
    }
}
