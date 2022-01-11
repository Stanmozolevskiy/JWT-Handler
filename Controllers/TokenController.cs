using JWT_Handler.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MyLibraries.JWT;

namespace JWT_Handler.Controllers
{
    [Route("[controller]"), ApiController, AllowAnonymous]
    public class TokenController: ControllerBase
    {
		[HttpPost("generate")]
		public async Task<IActionResult> GenerateTokne([FromBody] Token token)
		{
			await Task.Yield();
			return Ok(TokenFactory.Create(token.Role, token.UserId, token.Lifetime, token.Email, token.FirstName, token.LastName));
		}

		[HttpPost("decode")]
		public async Task<IActionResult> DecodeToken([FromBody] TokenTuple token)
		{
			await Task.Yield();
			IEnumerable<Claim> claims = TokenFactory.GetClaims(TokenFactory.GetToken(token.SecurityToken));
			return Ok(new Token()
			{
				Email = claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				FirstName = claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
				LastName = claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
				UserId = long.Parse(claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
				Role = claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
				Lifetime = TokenFactory.GetTokenLifetime(token.SecurityToken).Minutes
			});
		}
	}

	public class Token
	{
		public string Role { get; set; }
		public long UserId { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public long Lifetime { get; set; }
	}
}
