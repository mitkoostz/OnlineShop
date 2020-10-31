using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager){
        
              if(!userManager.Users.Any()){
                  var user = new AppUser{
                      DisplayName = "Bob",
                      UserName = "Bob",
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

                  await userManager.CreateAsync(user, "Pa$$w0rd");
              }
        }
    }
}