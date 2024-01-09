using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dashboard.Data.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(ApplicationDbContext context,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<List<AppUser>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                user.Roles = await GetUserRoles(user);
            }

            return users;
        }

        private async Task<List<IdentityRole>> GetUserRoles(AppUser user)
        {
            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = new List<IdentityRole>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(role);
                }
            }

            return roles;
        }
        //private async Task<List<string>> GetUserRoles(AppUser user)
        //{
        //    return (await _userManager.GetRolesAsync(user)).ToList();
        //}
        public AppUser GetUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }
            

        public ICollection<AppUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public AppUser UpdateUser(AppUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }
    }
}
