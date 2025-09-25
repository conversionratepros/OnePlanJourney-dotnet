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

        public List<SelectListItem> Plans { get; private set; } = new();
        public Dictionary<int, int> PlanPrices { get; private set; } = new();

        // Per-pet current add-on (for preselect)
        public Dictionary<int, string?> PetAddOns { get; private set; } = new();

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> AddOnOptions { get; set; } = new();

        private void BuildAddOnOptions()
        {
            AddOnOptions = new()
            {
                new("None", ""),
                new("Excess Buster", "Pay no excess when claiming for admission"),
                new("Pet Med Booster", "Add routine care & waive excess"),
                new("Diagnostic Booster", "Add diagnostic cover")
            };
        }

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
            public int? PlanType { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            BuildAddOnOptions();

            var leadExists = await _db.Leads.AnyAsync(l => l.Id == id);
            if (!leadExists)
            {
                TempData["Message"] = "Lead not found.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            var pets = await _db.Pets.Where(p => p.LeadId == id)
                                     .OrderBy(p => p.Id)
                                     .ToListAsync();
            if (pets.Count == 0)
            {
                TempData["Message"] = "No pets found for this lead.";
                return RedirectToPage("/Leads/PageTwo", new { id });
            }

            Plans = await _db.Plans.Where(pl => pl.IsActive)
                                   .OrderByDescending(pl => pl.PlanPrice)
                                   .Select(pl => new SelectListItem { Value = pl.Id.ToString(), Text = pl.PlanName })
                                   .ToListAsync();

            PlanPrices = await _db.Plans.Where(pl => pl.IsActive)
                                        .ToDictionaryAsync(pl => pl.Id, pl => pl.PlanPrice);

            PetAddOns = pets.ToDictionary(p => p.Id, p => p.AddOns);

            Input = new InputModel
            {
                LeadId = id,
                Items = pets.Select(p => new ItemRow
                {
                    PetId = p.Id,
                    Name = p.Name,
                    IsDog = p.IsDog,
                    PlanType = p.PlanType
                }).ToList()
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BuildAddOnOptions();

            if (!await _db.Leads.AnyAsync(l => l.Id == Input.LeadId))
                ModelState.AddModelError(string.Empty, "Lead not found.");

            if (!ModelState.IsValid)
            {
                Plans = await _db.Plans.Where(pl => pl.IsActive)
                                       .OrderByDescending(pl => pl.PlanPrice)
                                       .Select(pl => new SelectListItem { Value = pl.Id.ToString(), Text = pl.PlanName })
                                       .ToListAsync();
                PlanPrices = await _db.Plans.Where(pl => pl.IsActive)
                                            .ToDictionaryAsync(pl => pl.Id, pl => pl.PlanPrice);

                var petsForLead = await _db.Pets.Where(p => p.LeadId == Input.LeadId).ToListAsync();
                PetAddOns = petsForLead.ToDictionary(p => p.Id, p => p.AddOns);
                return Page();
            }

            // Save per-pet plan + add-on
            foreach (var row in Input.Items)
            {
                var pet = await _db.Pets.FirstOrDefaultAsync(p => p.Id == row.PetId && p.LeadId == Input.LeadId);
                if (pet is null) continue;

                pet.PlanType = row.PlanType;

                var selectedAddOn = Request.Form[$"addon_{row.PetId}"].ToString(); // radios named per pet
                pet.AddOns = string.IsNullOrWhiteSpace(selectedAddOn) || selectedAddOn == "None" ? null : selectedAddOn;
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = "Plans updated successfully.";
            return RedirectToPage("/Leads/PageFive", new { id = Input.LeadId });
        }
    }
}
