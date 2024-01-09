using Dashboard.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard.Areas.Admin.ViewModels
{
    public class EditUserViewModel
    {
        public AppUser User { get; set; }
        public IList<SelectListItem> Roles { get; set; }
    }
}
