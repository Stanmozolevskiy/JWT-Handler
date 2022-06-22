
namespace DataModels
{
	public class TokenRequest
	{
		public string token { get; set; }
	}
	public class Token
	{
		public long Intent { get; set; }
		public string Role { get; set; }
		public long UserId { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public long Lifetime { get; set; }
	}
}
