using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
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

        public async Task<string> LoginAsync(SignInModel signInModel)
        {
            SignInResult result = await signInManager
                .PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!result.Succeeded)
            {
                return null;
            }

            //if everything is fine, we have to add few claims in the application
            List<Claim> authClaims = new List<Claim>()
             {
                 new Claim(ClaimTypes.Name, signInModel.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            //then we have to add authentication key

            SymmetricSecurityKey authSigninKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));

            //now we have to generate new token

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                 issuer: configuration["JWT:ValidIssuer"],
                 audience: configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddDays(1),
                 claims: authClaims,
                 signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                 );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);            
        }
    }
}
