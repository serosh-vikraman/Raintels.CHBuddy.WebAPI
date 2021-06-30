using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace Raintels.CHBuddy.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyToken(TokenVerifyRequest request)
        {

            try
            {

                HttpClient client = new HttpClient();
                string encodedJwt = request.Token;
                // 1. Get Google signing keys
                client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
                HttpResponseMessage response = await client.GetAsync(
                  "x509/securetoken@system.gserviceaccount.com");
                if (!response.IsSuccessStatusCode) { return null; }
                var x509Data = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
                // 2. Configure validation parameters
                const string FirebaseProjectId = "chbuddy-e700f";
                var parameters = new TokenValidationParameters
                {
                    ValidIssuer = "https://securetoken.google.com/" + FirebaseProjectId,
                    ValidAudience = FirebaseProjectId,
                    IssuerSigningKeys = keys,
                };
                // 3. Use JwtSecurityTokenHandler to validate signature, issuer, audience and lifetime
                var handler = new JwtSecurityTokenHandler();
                SecurityToken token;
                ClaimsPrincipal principal = handler.ValidateToken(encodedJwt, parameters, out token);
                var jwt = (JwtSecurityToken)token;
                // 4.Validate signature algorithm and other applicable valdiations
                if (jwt.Header.Alg != SecurityAlgorithms.RsaSha256)
                {
                    throw new SecurityTokenInvalidSignatureException(
                      "The token is not signed with the expected algorithm.");
                }
            }
            catch (FirebaseException ex)
            {
                return BadRequest();
            }

            return BadRequest();
        }
        [HttpPost("verify1")]
        public async Task<bool> VerifyToken1(TokenVerifyRequest request)
        {
            var defaultAuth = FirebaseAuth.DefaultInstance;
            var decodedToken = await defaultAuth.VerifyIdTokenAsync(request.Token);
            return true;
        }
        private SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }
        [HttpGet("secrets")]
        [Authorize]
        public IEnumerable<string> GetSecrets()
        {
            return new List<string>()
            {
                "This is from the secret controller",
                "Seeing this means you are authenticated",
                "You have logged in using your google account from firebase",
                "Have a nice day!!"
            };
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