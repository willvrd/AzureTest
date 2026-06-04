using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication1.Modules.Blog.Posts.Entities;
using WebApplication1.Modules.Users.Entities;

namespace WebApplication1.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //CASE POSTS
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //CASE USER - OVERRIDE
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            });

            //CASE ROLE - OVERRIDE
            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");

                //CASE Permissions
                entity.Property(r => r.Permissions)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), //Setter
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>() //Getter
                    )
                    .HasColumnType("nvarchar(max)"); //Column Type
            });

            //CASE USER-ROLE - OVERRIDE
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));


        }
    }
}