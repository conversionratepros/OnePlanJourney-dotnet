using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Pages.Leads
{
    public class PageSixModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PageSixModel(ApplicationDbContext db) => _db = db;

        // Route id (/Leads/PageSix/{id})
        [BindProperty(SupportsGet = true)]
        public int LeadId { get; set; }

        // Address payload for either mode
        [BindProperty]
        public Address AddressInput { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            LeadId = id;

            // Prefill from existing address (if any)
            var existing = await _db.Address.FirstOrDefaultAsync(a => a.leadId == id);
            if (existing != null)
            {
                AddressInput = new Address
                {
                    Id = existing.Id,
                    leadId = existing.leadId,
                    PhysicalAddressLineOne = existing.PhysicalAddressLineOne,
                    PhysicalAddressLineTwo = existing.PhysicalAddressLineTwo,
                    PhysicalCity = existing.PhysicalCity,
                    PhysicalPostalCode = existing.PhysicalPostalCode,
                    DeliveryAddressLineOne = existing.DeliveryAddressLineOne,
                    DeliveryAddressLineTwo = existing.DeliveryAddressLineTwo,
                    DeliveryCity = existing.DeliveryCity,
                    DeliveryPostalCode = existing.DeliveryPostalCode
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var mode = Request.Form["Mode"].ToString(); // "old" or "tech"

            LeadId = id;
            if (AddressInput.leadId == 0) AddressInput.leadId = LeadId;

            // Required: physical fields (both modes)
            if (string.IsNullOrWhiteSpace(AddressInput.PhysicalAddressLineOne) ||
                string.IsNullOrWhiteSpace(AddressInput.PhysicalCity) ||
                string.IsNullOrWhiteSpace(AddressInput.PhysicalPostalCode))
            {
                ModelState.AddModelError(string.Empty, "Please complete the required physical address fields.");
            }

            // Tech mode requires delivery fields as well
            if (string.Equals(mode, "tech", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(AddressInput.DeliveryAddressLineOne) ||
                    string.IsNullOrWhiteSpace(AddressInput.DeliveryCity) ||
                    string.IsNullOrWhiteSpace(AddressInput.DeliveryPostalCode))
                {
                    ModelState.AddModelError(string.Empty, "Please complete the required delivery address fields for Tech Savvy.");
                }
            }
            else
            {
                // Old School => clear delivery fields
                AddressInput.DeliveryAddressLineOne = null;
                AddressInput.DeliveryAddressLineTwo = null;
                AddressInput.DeliveryCity = null;
                AddressInput.DeliveryPostalCode = null;
            }

            if (!ModelState.IsValid)
                return Page();

            // Upsert by leadId
            var existing = await _db.Address.FirstOrDefaultAsync(a => a.leadId == AddressInput.leadId);
            if (existing == null)
            {
                var entity = new Address
                {
                    leadId = AddressInput.leadId,
                    PhysicalAddressLineOne = AddressInput.PhysicalAddressLineOne,
                    PhysicalAddressLineTwo = AddressInput.PhysicalAddressLineTwo,
                    PhysicalCity = AddressInput.PhysicalCity,
                    PhysicalPostalCode = AddressInput.PhysicalPostalCode,
                    DeliveryAddressLineOne = AddressInput.DeliveryAddressLineOne,
                    DeliveryAddressLineTwo = AddressInput.DeliveryAddressLineTwo,
                    DeliveryCity = AddressInput.DeliveryCity,
                    DeliveryPostalCode = AddressInput.DeliveryPostalCode
                };
                _db.Address.Add(entity);
            }
            else
            {
                existing.PhysicalAddressLineOne = AddressInput.PhysicalAddressLineOne;
                existing.PhysicalAddressLineTwo = AddressInput.PhysicalAddressLineTwo;
                existing.PhysicalCity = AddressInput.PhysicalCity;
                existing.PhysicalPostalCode = AddressInput.PhysicalPostalCode;
                existing.DeliveryAddressLineOne = AddressInput.DeliveryAddressLineOne;
                existing.DeliveryAddressLineTwo = AddressInput.DeliveryAddressLineTwo;
                existing.DeliveryCity = AddressInput.DeliveryCity;
                existing.DeliveryPostalCode = AddressInput.DeliveryPostalCode;
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = "Address details saved.";
            // On to Page Seven with the same lead id
            return RedirectToPage("/Leads/PageSeven", new { id = LeadId });
        }
    }
}
