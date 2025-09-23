using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Models;

namespace OnePlanPetJourney.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Lead> Leads { get; set; } = null!;

    public DbSet<Pet> Pets => Set<Pet>();

    public DbSet<Plan> Plans => Set<Plan>();

}
