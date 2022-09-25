using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Movies_Api.Model
{
    public class ApplicationUser: IdentityUser
    {
       public string LastName { get; set; }  
       public string FirstName { get; set; }

    }
}
