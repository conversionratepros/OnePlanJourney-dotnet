using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Admin
{
    public class PagePetsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PagePetsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Pet> Pets { get; set; } = new List<Pet>();

        public async Task OnGetAsync()
        {
            Pets = await _context.Pets.ToListAsync();
        }
    }
}
