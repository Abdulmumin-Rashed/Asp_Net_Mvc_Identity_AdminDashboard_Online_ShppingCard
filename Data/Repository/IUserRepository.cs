using Dashboard.Models;

namespace Dashboard.Data.Repository
{
    public interface IUserRepository
    {
        ICollection<AppUser> GetUsers();
        AppUser GetUser(string id);
        AppUser UpdateUser(AppUser user);
        Task<List<AppUser>> GetAllUsersWithRolesAsync();
    }
}
