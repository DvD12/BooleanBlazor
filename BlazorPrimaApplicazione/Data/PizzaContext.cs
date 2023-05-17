using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorPrimaApplicazione.Data
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(OrmHelper.ConnectionString);
        }
    }
}
