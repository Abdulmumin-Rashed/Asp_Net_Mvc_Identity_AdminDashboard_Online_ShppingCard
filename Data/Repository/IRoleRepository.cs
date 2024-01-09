using Microsoft.AspNetCore.Identity;

namespace Dashboard.Data.Repository
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
