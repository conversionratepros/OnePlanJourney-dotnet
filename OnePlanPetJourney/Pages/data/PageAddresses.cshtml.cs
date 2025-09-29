using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Admin
{
    public class PageAddressesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PageAddressesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Address> Addresses { get; set; } = new List<Address>();

        public async Task OnGetAsync()
        {
            Addresses = await _context.Address
                .OrderBy(a => a.Id)
                .ToListAsync();
        }
    }
}
