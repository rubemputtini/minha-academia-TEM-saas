using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<BaseEquipment> BaseEquipments => Set<BaseEquipment>();
    public DbSet<Coach> Coaches => Set<Coach>();
    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<EquipmentSelection> EquipmentSelections => Set<EquipmentSelection>();
    public DbSet<Gym> Gyms => Set<Gym>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}