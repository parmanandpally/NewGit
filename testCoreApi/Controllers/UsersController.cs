using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using testCoreApi.Data;

namespace testCoreApi.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private IConfiguration _configuration;
		public UsersController(IConfiguration configuration)
        {
			this._configuration = configuration;
        }

		Dictionary<string, string> UsersRecords = new Dictionary<string, string>
		{
			{ "user1","password1"},
			{ "user2","password2"},
			{ "user3","password3"}
		};

		[HttpGet]
		[Authorize]
		public IActionResult Get()
		{
			//var users = new List<string> { "Anil kumar","Sunil Shastry","Rajesh Kumar","Amit Mishra", "Srinivasan" };
			//return users;
			efcoredbContext db = new efcoredbContext();
			return Ok(db.Employees.ToList());
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("{username}/{password}")]
		public IActionResult Authenticate(string username,string password)
		{
			if (!UsersRecords.Any(x => x.Key == username && x.Value == password))
			{
				return NotFound();
			}
			// below code generates JSON Web Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]   {
								new Claim(ClaimTypes.Name, username)
							}),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);//creates JWT token
			string newToken = tokenHandler.WriteToken(token);
			return Ok(newToken);
		}
	}

}