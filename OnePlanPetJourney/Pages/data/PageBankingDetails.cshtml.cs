using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Admin
{
    public class PageBankingDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PageBankingDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BankingDetails> BankingDetailsList { get; set; } = new List<BankingDetails>();

        public async Task OnGetAsync()
        {
            BankingDetailsList = await _context.BankingDetails
                .OrderBy(b => b.Id)
                .ToListAsync();
        }
    }
}
