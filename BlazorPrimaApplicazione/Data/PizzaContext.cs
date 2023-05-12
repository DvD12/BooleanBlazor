using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorPrimaApplicazione.Data
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PizzeriaEF;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
