using AdminGA.Entity.Student;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminGA.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GA_STDPREADMISSION> GA_STDPREADMISSION { get; set; }
        public DbSet<GA_STDCLASS> GA_STDCLASS { get; set; }
        public DbSet<GA_STDMEDIUM> GA_STDMEDIUM { get; set; }
        public DbSet<GA_STDUNQSERIES> GA_STDUNQSERIES { get; set; }

        public DbSet<GA_FEESMASTER> GA_FEESMASTER { get; set; }

    }
}