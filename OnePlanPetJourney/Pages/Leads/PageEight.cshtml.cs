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

        // Bind to your entity
        [BindProperty]
        public BankingDetails BankingDetails { get; set; } = new();

        public List<SelectListItem> BankNames { get; set; } = new();
        public List<SelectListItem> AccountTypes { get; set; } = new();
        public List<SelectListItem> HearAboutUsOptions { get; set; } = new();

        private void BuildSelectLists()
        {
            BankNames = new List<SelectListItem>
            {
                new("FNB","FNB"),
                new("Absa","Absa"),
                new("Standard Bank","Standard Bank"),
                new("Nedbank","Nedbank"),
                new("Capitec","Capitec"),
            };

            AccountTypes = new List<SelectListItem>
            {
                new("Cheque","Cheque"),
                new("Savings","Savings"),
                new("Transmission","Transmission"),
            };

            HearAboutUsOptions = new List<SelectListItem>
            {
                new("Friend","Friend"),
                new("Social Media","Social Media"),
                new("Google","Google"),
                new("Other","Other"),
            };
        }

        // Accept the route id and prefill leadId
        public void OnGet(int id)
        {
            BuildSelectLists();
            BankingDetails = new BankingDetails
            {
                leadId = id
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BuildSelectLists();

            // Ensure lead id is present (in case model binding missed it)
            if (BankingDetails.leadId == 0 && int.TryParse(RouteData.Values["id"]?.ToString(), out var rid))
            {
                BankingDetails.leadId = rid;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BankingDetails.Add(BankingDetails);
            await _context.SaveChangesAsync();

            // NOTE: property is leadId (lowercase 'l'), not LeadId
            return RedirectToPage("/Leads/PageNine", new { id = BankingDetails.leadId });
        }
    }
}
