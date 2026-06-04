using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Modules.Users.Entities
{
    public class Role : IdentityRole
    {
      
        //NVARCHAR/JSON.
        public List<string> Permissions { get; set; } = new List<string>();

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}