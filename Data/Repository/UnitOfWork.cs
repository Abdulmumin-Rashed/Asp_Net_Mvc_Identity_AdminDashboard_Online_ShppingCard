using Dashboard.Models;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;        
        public ICategoryRepository Category { get; private set; }
        //public ICartRepository Cart { get; private set; }
        public IRoleRepository Role { get; private set; }
        public IProductRepository Product { get; private set; }
        public IUserRepository User { get; private set; }

        public ICartRepository Cart { get; private set; }

        public UnitOfWork(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            Role = new RoleRepository(context);
            User = new UserRepository(context, userManager, roleManager);
            Cart = new CartRepository(_context);



        }
        public void Save()
        {
            _context.SaveChanges();
        }

      
    }
}
