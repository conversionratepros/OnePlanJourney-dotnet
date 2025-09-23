using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    // Optional: show recent leads on the homepage
    public async Task<IActionResult> Index()
    {
        var leads = await _db.Leads
            .OrderByDescending(l => l.Id)
            .Take(20)
            .ToListAsync();

        return View(leads); // If you prefer, return View(); and ignore this list in the view.
    }

    // GET: /Home/CreateLead
    [HttpGet]
    public IActionResult CreateLead()
    {
        return View();
    }

    // POST: /Home/CreateLead
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLead(Lead model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _db.Leads.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Lead created successfully.";
        return RedirectToAction(nameof(Index));
    }


   [HttpGet]
    public IActionResult PageOne()
    {
        return View();
    }

    // POST: /Home/PageOne
 [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> PageOne([Bind("CatCount,DogCount")] Lead input)
{
    // 1) Create the Lead first
    var lead = new Lead
    {
        CatCount = input.CatCount,
        DogCount = input.DogCount
    };

    _db.Leads.Add(lead);
    await _db.SaveChangesAsync(); // sets lead.Id

    // Common defaults that satisfy required fields
    DateTime defaultDob = DateTime.Today; // within 1900..2100 range
    const string unknown = "Unknown";
    const string none    = "None";

    // 2) Add Pet entries with valid placeholder values
    // Cats
    for (int i = 0; i < lead.CatCount; i++)
    {
        _db.Pets.Add(new Pet
        {
            LeadId           = lead.Id,
            Name             = $"Cat {i + 1}",
            DOB              = defaultDob,
            IsMale           = false,               // placeholder; user can change later
            IsDog            = false,               // cat
            Breed            = unknown,
            Colour           = unknown,
            ChipNumber       = string.Empty,        // optional
            PreferredVet     = unknown,
            IsNeutered       = false,               // placeholder
            Injuries         = none,
            MedicalCondition = none
        });
    }

    // Dogs
    for (int i = 0; i < lead.DogCount; i++)
    {
        _db.Pets.Add(new Pet
        {
            LeadId           = lead.Id,
            Name             = $"Dog {i + 1}",
            DOB              = defaultDob,
            IsMale           = false,               // placeholder; user can change later
            IsDog            = true,                // dog
            Breed            = unknown,
            Colour           = unknown,
            ChipNumber       = string.Empty,        // optional
            PreferredVet     = unknown,
            IsNeutered       = false,               // placeholder
            Injuries         = none,
            MedicalCondition = none
        });
    }

    await _db.SaveChangesAsync();

    TempData["Message"] = $"Lead {lead.Id} created with {lead.CatCount} cat(s) and {lead.DogCount} dog(s).";
    return RedirectToAction("PageTwo", new { id = lead.Id });
}


   // GET: /Home/PageTwo/{id}
    [HttpGet]
    public async Task<IActionResult> PageTwo(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        return View(lead); // strongly typed view with Lead model
    }

    // POST: /Home/PageTwo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PageTwo([Bind("Id,FirstName,LastName,DOB,Province,Email")] Lead input)
    {
        var lead = await _db.Leads.FirstOrDefaultAsync(l => l.Id == input.Id);
        if (lead == null) return NotFound();

        // ✅ Only update the fields we care about
        lead.FirstName = input.FirstName;
        lead.LastName  = input.LastName;
        lead.DOB       = input.DOB;
        lead.Province  = input.Province;
        lead.Email     = input.Email;

        // Do NOT touch CatCount / DogCount

        await _db.SaveChangesAsync();

        TempData["Message"] = "Lead details updated successfully.";
        return RedirectToAction("AddPetDetails", "Pet", new { LeadId = lead.Id });

    }


     public IActionResult PageThree()
    {
        return View();
    }

     public IActionResult PageFour()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
