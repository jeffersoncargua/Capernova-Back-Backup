using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ManagmentController : ControllerBase
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        
        public ManagmentController(ApplicationDbContext db)
        {
            //_userManager = userManager;
            //_roleManager = roleManager;
            
        }

    }
}
