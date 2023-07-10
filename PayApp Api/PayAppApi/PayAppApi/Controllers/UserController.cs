using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PayAppApi.Common.Helpers;
using PayAppApi.Data;
using PayAppApi.Models;
using PayAppApi.ViewModels;
using PayAppApi.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly PayAppDbContext _authcontext;
        private readonly IConfiguration _configuration;
        public UserController(PayAppDbContext payAppDbContext, IConfiguration configuration)
        {
            _authcontext = payAppDbContext;
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<TokenResponse>> Autheticate([FromBody] AuthenticateRequest request)
        {
            if (request is null)
                return BadRequest();

            var user = await _authcontext.Users.FirstOrDefaultAsync(x => x.Phone == request.Phone);
            if (user is null)
                return NotFound(new { Message = "User not found." });

            var isPasswordMatch = StaticData.ComparePassword(request.Password, user.Password);
            if (!isPasswordMatch)
                return BadRequest(new { Message = "Password did not matched." });

            var userClaims = GetClaims(user);
            var securityToken = GenerateToken(userClaims);


            var tokenDetails = new TokenResponse
            {
                StatusCode = 0,
                Message = "Login Sucess",
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                ExpiryDate = securityToken.ValidTo.ToString()
            };

            return Ok(tokenDetails);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> RegisterUser([FromBody] User request)
        {
            if (request is null)
                return BadRequest();


            var user = await _authcontext.Users.Where(x => x.Phone == request.Phone).FirstOrDefaultAsync();
            if (user is not null)
                return BadRequest(new ApiResponse
                {
                    StatusCode = 1001,
                    Message = "User has been already registered with this phone number"
                });

            request.Password = StaticData.ComputeSHA256(request.Password);

            _authcontext.Users.Add(request);
            await _authcontext.SaveChangesAsync();
            return Ok(new ApiResponse
            {
                StatusCode = 0,
                Message = "User Registered"
            });

        }

        [HttpGet("get-user-dropdown")]
        [Authorize]
        public async Task<ActionResult<List<UserDropDownModel>>> GetUserDropDown()
        {
            var users = await _authcontext.Users.Select(x => new UserDropDownModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return Ok(users);
        }

        private List<Claim> GetClaims(User identityUser)
        {
            var claims = new List<Claim>
            {
               new Claim(CustomConstants.Name, identityUser.Name),
                new Claim(CustomConstants.Email, identityUser.Email),
                new Claim(CustomConstants.Phone, identityUser.Phone),
                new Claim(CustomConstants.Id, identityUser.Id.ToString()),
            };
            return claims;
        }
        private JwtSecurityToken GenerateToken(List<Claim> userClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenDefination:JwtKey"]));

            return new JwtSecurityToken(issuer: _configuration["TokenDefination:JwtIssuer"],
                                                           audience: _configuration["TokenDefination:JwtAudience"],
                                                           claims: userClaims,
                                                           expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["TokenDefination:JwtValidMinutes"])),
                                                           signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
        }
    }
}
