using MyLibraries;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using static MyLibraries.JWT;

namespace Utility.Helpers
{
    public class TokenHelper
	{
       
		public static string Create(string role, long userId, long intent, long lifetime, string email, string firstName, string lasetName)
		{
			const string Subject = "Subject";
			const string Issuer = "Issuer";
			const string Audience = "Audience";
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userId)),
				new Claim(ClaimTypes.Role, role),
				new Claim(ClaimTypes.Email , email),
				new Claim(ClaimTypes.Surname , lasetName),
				new Claim(ClaimTypes.GivenName , firstName),
				new Claim("Intent" , Convert.ToString(intent))
			};
			return JWT.CreateToken(Subject, Issuer, Audience, TimeSpan.FromMinutes(lifetime), claims).SecurityToken;
		}

		public static IEnumerable<Claim> GetClaims(TokenTuple token) => JWT.GetClaims(token);
		public static TokenTuple GetToken(string token) => JWT.GetToken(token);
		public static TimeSpan GetTokenLifetime(string token) => JWT.GetLifetime(token);

	}

}
