using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models; // Pet, Lead, PetListViewModel
using Microsoft.AspNetCore.Mvc.Rendering;


namespace OnePlanPetJourney.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PetController(ApplicationDbContext db) => _db = db;



          public async Task<IActionResult> Index()
        {
            var pets = await _db.Pets
                .OrderBy(p => p.Id)
                .ToListAsync();

            return View(pets);
        }

        // GET: /Pet/AddPetDetails?leadId=123
        [HttpGet]
        public async Task<IActionResult> AddPetDetails(int leadId)
        {
            // ensure the lead exists
            var leadExists = await _db.Leads.AnyAsync(l => l.Id == leadId);
            if (!leadExists)
            {
                TempData["Message"] = "Owner not found.";
                return RedirectToAction("PageTwo", "Home", new { id = leadId });
            }

            var pets = await _db.Pets
                .Where(p => p.LeadId == leadId)
                .OrderBy(p => p.Id)
                .ToListAsync();

            if (!pets.Any())
            {
                TempData["Message"] = "No pets found for this lead.";
                return RedirectToAction("PageTwo", "Home", new { id = leadId });
            }

            var vm = new PetListViewModel
            {
                LeadId = leadId,
                Pets   = pets
            };

            return View(vm); // Views/Pet/AddPetDetails.cshtml
        }

        // POST: /Pet/AddPetDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPetDetails(PetListViewModel vm)
        {
            // validate lead
            if (!await _db.Leads.AnyAsync(l => l.Id == vm.LeadId))
                ModelState.AddModelError(string.Empty, "Owner not found.");

            if (!ModelState.IsValid)
                return View(vm);

            // update existing pets (whitelist)
            foreach (var pet in vm.Pets)
            {
                var existing = await _db.Pets
                    .FirstOrDefaultAsync(p => p.Id == pet.Id && p.LeadId == vm.LeadId);

                if (existing is null) continue;

                existing.Name             = pet.Name;
                existing.DOB              = pet.DOB;
                existing.IsMale           = pet.IsMale;
                existing.IsDog            = pet.IsDog;
                existing.Breed            = pet.Breed;
                existing.Colour           = pet.Colour;
                existing.ChipNumber       = pet.ChipNumber;
                existing.PreferredVet     = pet.PreferredVet;
                existing.IsNeutered       = pet.IsNeutered;
                existing.Injuries         = pet.Injuries;
                existing.MedicalCondition = pet.MedicalCondition;
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = "Pet details saved successfully.";

            // reload fresh data and stay on the same page
            var refreshed = new PetListViewModel
            {
                LeadId = vm.LeadId,
                Pets   = await _db.Pets.Where(p => p.LeadId == vm.LeadId).OrderBy(p => p.Id).ToListAsync()
            };

        return RedirectToAction("SetPetPlans", "Pet", new { leadId = vm.LeadId });

        }


         // GET: /Pet/SetPetPlans?leadId=123
        [HttpGet]
        public async Task<IActionResult> SetPetPlans(int leadId)
        {
            var leadExists = await _db.Leads.AnyAsync(l => l.Id == leadId);
            if (!leadExists)
            {
                TempData["Message"] = "Lead not found.";
                return RedirectToAction("PageTwo", "Home", new { id = leadId });
            }

            var pets = await _db.Pets
                .Where(p => p.LeadId == leadId)
                .OrderBy(p => p.Id)
                .ToListAsync();

            if (pets.Count == 0)
            {
                TempData["Message"] = "No pets found for this lead.";
                return RedirectToAction("PageTwo", "Home", new { id = leadId });
            }

            var plans = await _db.Plans
                .Where(pl => pl.IsActive)
                .OrderBy(pl => pl.PlanName)
                .Select(pl => new SelectListItem
                {
                    Value = pl.Id.ToString(),
                    Text  = pl.PlanName
                })
                .ToListAsync();

            var vm = new SetPetPlansViewModel
            {
                LeadId = leadId,
                Plans  = plans,
                Items  = pets.Select(p => new PetPlanItem
                {
                    PetId    = p.Id,
                    Name     = p.Name,
                    IsDog    = p.IsDog,
                    PlanType = p.PlanType  // current selection (may be null)
                }).ToList()
            };

            return View(vm);
        }

        // POST: /Pet/SetPetPlans
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPetPlans(SetPetPlansViewModel vm)
        {
            // Make sure lead exists
            if (!await _db.Leads.AnyAsync(l => l.Id == vm.LeadId))
            {
                ModelState.AddModelError("", "Lead not found.");
            }

            if (!ModelState.IsValid)
            {
                // rehydrate the plans dropdown if we redisplay the page
                vm.Plans = await _db.Plans
                    .Where(pl => pl.IsActive)
                    .OrderBy(pl => pl.PlanName)
                    .Select(pl => new SelectListItem
                    {
                        Value = pl.Id.ToString(),
                        Text  = pl.PlanName
                    })
                    .ToListAsync();

                return View(vm);
            }

            // Update each pet's PlanType
            foreach (var row in vm.Items)
            {
                var pet = await _db.Pets.FirstOrDefaultAsync(p => p.Id == row.PetId && p.LeadId == vm.LeadId);
                if (pet is null) continue;

                pet.PlanType = row.PlanType; // may be null or a valid Plan Id
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = "Plans updated successfully.";

            // Stay on the same page with refreshed data
            return RedirectToAction(nameof(SetPetPlans), new { leadId = vm.LeadId });
        }
    }
}
