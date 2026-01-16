using Customer.Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure;

public class CustomerReadDbContext(DbContextOptions<CustomerReadDbContext> options) : DbContext(options)
{
    public DbSet<CustomerReadModel> CustomerReadModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerReadModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName);
            entity.Property(e => e.PhoneNumber).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.BillingAddress);
            entity.Property(e => e.DateOfBirth);
        });
    }
}