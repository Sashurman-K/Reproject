using Microsoft.AspNetCore.Mvc;
using Reproject.Configures;
using Reproject.Models.Context;
using Reproject.Processors;
using System.Security.Claims;
using Reproject.Models.Requests;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Reproject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AuthOptions _authOptions;
        private readonly NoteDbContext _dbContext;
        private readonly ClaimsIdentityGenerator _identityGenerator;

        public HomeController(AuthOptions authOptions, NoteDbContext dbContext, ClaimsIdentityGenerator identityGenerator)
        {
            _authOptions = authOptions;
            _dbContext = dbContext;
            _identityGenerator = identityGenerator;
        }
        [HttpGet("/getsalt")]
        public ActionResult GetSalt([FromBody] UserRequest userRequest) { }

        [HttpPost("/login")]
        public IActionResult LogIn([FromBody] UserRequest userRequest)
        {
            User user = _identityGenerator.GetUserOrDefault(userRequest.Login, userRequest.Password);
            var encodedJwt = _identityGenerator.GetJwt(user, out ClaimsIdentity identity);
            if (user == null)
            {
                return BadRequest(new { errorText = "invalid user" });
            }
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return new JsonResult(response);
        }

        [HttpPost("/signup")]
        public IActionResult SignUp([FromBody] UserRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.Login) || string.IsNullOrWhiteSpace(userRequest.Password) || string.IsNullOrWhiteSpace(userRequest.UserName))
            {
                return BadRequest(new { errorText = "Empty parametrs" });
            }
            if (_dbContext.UserDbSet.Any(item => item.UserLogin == userRequest.Login))
            {
                return BadRequest(new { errorText = "Login used" });
            }
            User user = new User()
            {
                Id = Guid.NewGuid(),
                UserPassword = PasswordHasher.Hash(userRequest.Password),
                UserName = userRequest.UserName,
                UserLogin = userRequest.Login,
            };
            _dbContext.UserDbSet.Add(user);
            _dbContext.SaveChanges();
            var encodedJwt = _identityGenerator.GetJwt(user, out ClaimsIdentity identity);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return new JsonResult(response);
        }
    }
}
