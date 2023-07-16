using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            ApplicationUser user = new ApplicationUser 
            { 
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                UserName = signUpModel.Email, 
                Email = signUpModel.Email,                 
            };
            return await userManager.CreateAsync(user, signUpModel.Password);
        }
    }
}
