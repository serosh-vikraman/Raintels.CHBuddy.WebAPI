using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace Raintels.CHBuddy.Web.API.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("verify")]
        public async Task<ResponseDataModel<string>> VerifyToken()
        {
            try
            {
                var idToken = HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "Authorization").Value;
                var token = idToken.ToString().Replace("Bearer", "").Trim();
                var defaultAuth = FirebaseAuth.DefaultInstance;
                var decodedToken = await defaultAuth.VerifyIdTokenAsync(token);
                if (decodedToken == null || decodedToken.Claims == null ||
                   string.IsNullOrEmpty(decodedToken.Claims["email"].ToString()))
                {
                    throw new Exception("Unauthorized");
                }
                var email = decodedToken.Claims["email"].ToString();
                var googleId = decodedToken.Uid;
                UserModel user = new UserModel()
                {
                    Email = email,
                    GoogleID = googleId
                };
                var userId = _userService.CreateUser(user);
                var response = new ResponseDataModel<string>()
                {
                    Status = HttpStatusCode.OK,
                    Message = "Success",
                    Response = userId.ToString()
                };
                return response;
            }
            catch (FirebaseException ex)
            {

                if (ex.Message.Contains("Unauthorized"))
                {
                    return new ResponseDataModel<string>()
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Failure",
                        Response = "0"
                    };
                }
                else
                {
                    return new ResponseDataModel<string>()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = "Failure",
                        Response = "0"
                    };
                }

            }

        }


        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }


        [HttpGet("secrets")]
        [Authorize]
        public string GetSecrets()
        {
            return "Have a nice day!!";
        }
        [HttpGet("test")]
        public string Test()
        {
            return "Have a nice day!!";
        }

    }
    public class TokenVerifyRequest
    {
        public string Token { get; set; }
    }
}