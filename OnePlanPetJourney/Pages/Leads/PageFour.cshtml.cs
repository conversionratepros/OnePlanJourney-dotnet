using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;
using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageFourModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageFourModel(ApplicationDbContext db) => _db = db;

        // Plans to render (like your MVC 'plans' SelectListItems)
        public List<SelectListItem> Plans { get; private set; } = new();

        public Dictionary<int, int> PlanPrices { get; private set; } = new();


        // Bindable form payload
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public int LeadId { get; set; }

            public List<ItemRow> Items { get; set; } = new();
        }

        public class ItemRow
        {
            public int PetId { get; set; }
            public string Name { get; set; } = string.Empty;
            public bool IsDog { get; set; }
            public int? PlanType { get; set; } // selected Plan Id (nullable)
        }

        // GET /Leads/PageFour/{id}
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Lead exists?
            var leadExists = await _db.Leads.AnyAsync(l => l.Id == id);
            if (!leadExists)
            {
                TempData["Message"] = "Lead not found.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            // Pets for this lead
            var pets = await _db.Pets
                .Where(p => p.LeadId == id)
                .OrderBy(p => p.Id)
                .ToListAsync();

            if (pets.Count == 0)
            {
                TempData["Message"] = "No pets found for this lead.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            // Active plans
           Plans = await _db.Plans
            .Where(pl => pl.IsActive)
            .OrderByDescending(pl => pl.PlanPrice)     
            .Select(pl => new SelectListItem
            {
                Value = pl.Id.ToString(),
                Text  = pl.PlanName
            })
            .ToListAsync();                             

        // 2) Load the price map (id -> price)
            PlanPrices = await _db.Plans
                .Where(pl => pl.IsActive)
                .ToDictionaryAsync(pl => pl.Id, pl => pl.PlanPrice); 

            Input = new InputModel
            {
                LeadId = id,
                Items = pets.Select(p => new ItemRow
                {
                    PetId    = p.Id,
                    Name     = p.Name,
                    IsDog    = p.IsDog,
                    PlanType = p.PlanType
                }).ToList()
            };

            return Page();
        }

        // POST
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate lead
            if (!await _db.Leads.AnyAsync(l => l.Id == Input.LeadId))
            {
                ModelState.AddModelError(string.Empty, "Lead not found.");
            }

            if (!ModelState.IsValid)
            {
                // Rehydrate plans for redisplay
                Plans = await _db.Plans
                    .Where(pl => pl.IsActive)
                    .OrderBy(pl => pl.PlanName)
                    .Select(pl => new SelectListItem
                    {
                        Value = pl.Id.ToString(),
                        Text  = pl.PlanName
                    })
                    .ToListAsync();

                return Page();
            }

            // Update each pet's PlanType
            foreach (var row in Input.Items)
            {
                var pet = await _db.Pets.FirstOrDefaultAsync(p => p.Id == row.PetId && p.LeadId == Input.LeadId);
                if (pet is null) continue;

                pet.PlanType = row.PlanType; // may be null or a valid Plan Id
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = "Plans updated successfully.";

            // Stay on same page with refreshed data
            return RedirectToPage("/Leads/PageFour", new { id = Input.LeadId });
        }
    }
}
