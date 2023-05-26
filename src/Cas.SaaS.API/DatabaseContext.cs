using Cas.SaaS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cas.SaaS.API;

public class DatabaseContext : DbContext
{
    #region Tables
    public DbSet<Application> Applications { get; set; } = null!;
    public DbSet<Brigade> Brigades { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!; 
    public DbSet<Delivery> Deliveries { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<TariffPlan> TariffPlans { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    #endregion

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public DatabaseContext() { }

    /// <summary>
    /// Конструктор с опциями
    /// </summary>
    /// <param name="options"></param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Surname).IsRequired();
            entity.Property(e => e.Patronymic);

            entity.Property(e => e.Login).IsRequired();
            entity.Property(e => e.Phone).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Role).IsRequired();

            entity.Property(e => e.RefreshToken).IsRequired(false);
            entity.Property(e => e.RefreshTokenExpires).IsRequired(false);

            entity.HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Role = UserRoles.Admin,
                Login = "pmarkelo77",
                Password = "pmarkelo77",
                Phone = "79887774433",
                Email = "pmarkelo77@gmail.com",
                Name = "Павел",
                Surname = "Маркелов",
            });
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description);
            entity.Property(e => e.Phone).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.Status).IsRequired();
        });

        modelBuilder.Entity<Brigade>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Customer).IsRequired();
            entity.Property(e => e.Phone).IsRequired();
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.Description).IsRequired();

            entity.HasOne(b => b.Service).WithMany(s => s.Brigades).HasForeignKey(b => b.ServiceId);
            entity.HasMany(b => b.Employees).WithMany(e => e.Brigades);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Status).IsRequired();

            entity.HasMany(c => c.Employees).WithOne(e => e.Client).HasForeignKey(e => e.ClientId);
            entity.HasMany(c => c.Deliveries).WithOne(d => d.Client).HasForeignKey(d => d.ClientId);
            entity.HasMany(c => c.Services).WithOne(s => s.Client).HasForeignKey(s => s.ClientId);
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();

            entity.HasOne(d => d.Client).WithMany(c => c.Deliveries).HasForeignKey(d => d.ClientId);
            entity.HasOne(d => d.TariffPlan).WithMany(t => t.Deliveries).HasForeignKey(d => d.TariffPlanId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.IsActive).IsRequired();

            entity.HasOne(e => e.Client).WithMany(c => c.Employees).HasForeignKey(e => e.ClientId);
            entity.HasMany(e => e.Brigades).WithMany(b => b.Employees);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Tools).IsRequired();

            entity.HasOne(s => s.Client).WithMany(c => c.Services).HasForeignKey(s => s.ClientId);
            entity.HasMany(s => s.Brigades).WithOne(b => b.Service).HasForeignKey(b => b.ServiceId);
        });

        modelBuilder.Entity<TariffPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Payment).IsRequired();
            entity.Property(e => e.Price).IsRequired();
            entity.Property(e => e.Description);
            entity.Property(e => e.CountEmployees).IsRequired();

            entity.HasMany(t => t.Deliveries).WithOne(d => d.TariffPlan).HasForeignKey(d => d.TariffPlanId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
