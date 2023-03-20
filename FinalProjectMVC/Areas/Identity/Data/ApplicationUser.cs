using Microsoft.AspNetCore.Identity;

namespace FinalProjectMVC.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {     
        /*
         Anything added to this class will be migrated into the 
         `Asp.netUsers table in the database. 
        */
        public string? Name { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
