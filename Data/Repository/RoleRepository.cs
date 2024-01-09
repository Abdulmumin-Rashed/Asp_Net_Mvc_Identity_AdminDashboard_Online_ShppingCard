using Microsoft.AspNetCore.Identity;

namespace Dashboard.Data.Repository
{
    public class RoleRepository: IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
