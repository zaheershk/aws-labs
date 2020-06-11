using System.Threading.Tasks;
using WebAdvert.App.Models;
using WebAdvert.App.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using System.Linq;

namespace WebAdvert.App.Data
{
    public class AuthService
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;

        public AuthService(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoUserPool = cognitoUserPool;
        }

        public async Task<ServiceResponse<bool>> RegisterUser(SignUpModel model)
        {
            // fetch user
            var user = _cognitoUserPool.GetUser(model.Email);

            // add attributes
            user.Attributes.Add(CognitoAttribute.Name.ToString(), model.Name);

            // create user
            var createdResult = await _userManager.CreateAsync(user, model.Password);
            return createdResult.Succeeded
                ? new ServiceResponse<bool>(true, "User created!")
                : new ServiceResponse<bool>(false, createdResult.Errors.Select(x => x.Description).ToList());
        }

        public async Task<ServiceResponse<bool>> ConfirmUser(ConfirmModel model)
        {
            // fetch user
            var user = await _userManager.FindByIdAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse<bool>(false, $"User with {model.Email} email address was not found!");
            }

            // confirm user
            var confirmResult = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, model.Code, true).ConfigureAwait(false);
            return confirmResult.Succeeded
                ? new ServiceResponse<bool>(true, "User confirmed!")
                : new ServiceResponse<bool>(false, confirmResult.Errors.Select(x => x.Description).ToList());
        }

        public async Task<ServiceResponse<bool>> LoginUser(SignInViewModel model)
        {
            // fetch user
            var user = await _userManager.FindByIdAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse<bool>(false, $"User with {model.Email} email address was not found!");
            }

            // confirm user
            var loginResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).ConfigureAwait(false);
            return loginResult.Succeeded
                ? new ServiceResponse<bool>(true, "User logged in!")
                : new ServiceResponse<bool>(false, "Login failed!");
        }
    }
}
