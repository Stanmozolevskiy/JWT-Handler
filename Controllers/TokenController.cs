﻿using Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataModels;

namespace Utility.Controllers
{
    [Route("[controller]"), ApiController, AllowAnonymous]
    public class TokenController: ControllerBase
    {
		[HttpPost("generate")]
		public async Task<IActionResult> GenerateTokne([FromBody] Token token)
		{
			await Task.Yield();
			return Ok(TokenHelper.Create(token.Role, token.UserId, token.Intent, token.Lifetime, token.Email, token.FirstName, token.LastName));
		}

		[HttpPost("decode")]
		public async Task<IActionResult> DecodeToken([FromBody] TokenRequest token)
		{
			await Task.Yield();
			IEnumerable<Claim> claims = TokenHelper.GetClaims(TokenHelper.GetToken(token.token));
			return Ok(new Token()
			{
				Email = claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				FirstName = claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
				LastName = claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
				UserId = long.Parse(claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
				Role = claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
				Intent = long.Parse(claims.SingleOrDefault(c => c.Type == "Intent")?.Value),
				Lifetime = TokenHelper.GetTokenLifetime(token.token).Minutes
			});
		}
	}


}