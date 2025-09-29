using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageNineModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageNineModel(ApplicationDbContext db) => _db = db;

        [BindProperty(SupportsGet = true)]
        public int LeadId { get; set; }

        public List<Pet> Pets { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            LeadId = id;
            Pets = await _db.Pets.Where(p => p.LeadId == id).ToListAsync();
            return Page();
        }
    }
}
