using Microsoft.EntityFrameworkCore;
using TestWebApi.Models;
using TestWebApi.Models.ResponseModels;

namespace TestWebApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> products { get; set; }
    }
}
