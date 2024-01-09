using Dashboard.Areas.Admin.ViewModels;
using Dashboard.Data.Repository;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Super_Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepository _userRepository;
        public UserController(IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }



        public async Task<IActionResult> Index()
        {
            var usersWithRoles = await _userRepository.GetAllUsersWithRolesAsync();
            return View(usersWithRoles);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);
            var roleItems = roles.Select(role => new SelectListItem(
                role.Name,
                role.Id,
                userRoles.Any(ur => ur.Contains(role.Name)))).ToList();

            var vm = new EditUserViewModel { User = user, Roles = roleItems };
            return View(vm);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel data)
        {
            var user = _unitOfWork.User.GetUser(data.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            //Loop through the roles in ViewModel
            //Check if the Role is Assigned In DB
            //If Assigned -> Do Nothing
            //If Not Assigned -> Add Role

            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();

            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToDelete.Add(role.Text);
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.FirstName = data.User.FirstName;
            user.LastName = data.User.LastName;
            

            _unitOfWork.User.UpdateUser(user);

            return RedirectToAction("Index", "User");
        }
    }
}
