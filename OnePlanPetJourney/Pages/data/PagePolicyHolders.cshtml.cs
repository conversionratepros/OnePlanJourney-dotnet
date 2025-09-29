using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Admin
{
    public class PagePolicyHoldersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PagePolicyHoldersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PolicyHolder> PolicyHolders { get; set; } = new List<PolicyHolder>();

        public async Task OnGetAsync()
        {
            PolicyHolders = await _context.PolicyHolder
                .OrderBy(ph => ph.LastName)
                .ToListAsync();
        }
    }
}
