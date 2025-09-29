using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Admin
{
    public class PageLeadsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PageLeadsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Lead> Leads { get; set; } = new List<Lead>();

        public async Task OnGetAsync()
        {
            Leads = await _context.Leads
                .Include(l => l.Pets) // eager load pets so Count works
                .ToListAsync();
        }
    }
}
