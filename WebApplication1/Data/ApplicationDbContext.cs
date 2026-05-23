using Microsoft.EntityFrameworkCore;
using WebApplication1.Modules.Posts.Entities;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Register Entity
        public DbSet<Post> Posts { get; set; }
    }
}
