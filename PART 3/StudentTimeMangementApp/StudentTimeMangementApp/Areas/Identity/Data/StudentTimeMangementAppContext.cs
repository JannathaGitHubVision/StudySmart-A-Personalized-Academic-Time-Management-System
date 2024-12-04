using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentTimeMangementApp.Areas.Identity.Data;
using StudentTimeMangementApp.Models;

namespace StudentTimeMangementApp.Data;

public class StudentTimeMangementAppContext : IdentityDbContext<RegistrationLoginUser>
{
    public StudentTimeMangementAppContext(DbContextOptions<StudentTimeMangementAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Modules>? Modules { get; set; }

    public DbSet<DisplayValues>? DisplayValues { get; set; }
}
