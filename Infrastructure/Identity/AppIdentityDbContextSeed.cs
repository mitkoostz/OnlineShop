using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager){
        
              if(!userManager.Users.Any()){
                  var user = new AppUser{
                      DisplayName = "Bob",
                      UserName = "bob@test.com",
                      Email = "bob@test.com",
                      Address = new Address{
                          FirstName = "Bob",
                          LastName = "Bobitty",
                          Street = " 10 the street",
                          City = "New york",
                          State = "NY",
                          Zipcode ="90210"
                      }
                  };
                  IdentityRole admin = new IdentityRole()
                  {
                      Name = "Admin"
                  };
                  await roleManager.CreateAsync(admin);
                  await userManager.AddToRoleAsync(user,"Admin");
                  await userManager.CreateAsync(user, "Pa$$w0rd");
              }
        }
    }
}