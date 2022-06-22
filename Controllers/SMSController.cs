using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyLibraries;
using System.Threading.Tasks;
using Twilio;
using System.Net;
using Microsoft.Extensions.Configuration;
using Firebase.Auth;

namespace Utility.Controllers
{
    [Route("[controller]"), ApiController, AllowAnonymous]
    public class SMSController : ControllerBase
    {
        public SMSController(IConfiguration configuration)
        {
			this.configuration = configuration;
			accountSid = configuration["Settings:Tvilio:accountSid"];
			authToken = configuration["Settings:Tvilio:authToken"];
		}

		[HttpPost("saveAttachment")]
		public async Task<IActionResult> SaveAttachment(List<IFormFile> files)
		{
			foreach (IFormFile file in files)
				await Helpers.FileHelper.CopyFileLocally(file.OpenReadStream(),
					$"{System.IO.Directory.CreateDirectory("Attachments").Name}/{file.FileName}");

			return Ok();
		}

		[HttpPost("rempveAttachment")]
		public async Task<IActionResult> RempveAttachment(IFormCollection fileNames)=>
			Ok(await Helpers.FileHelper.DeleteFileLocally(fileNames["fileNames"].Single()));
		
		[HttpPost("send")]
        public async Task<IActionResult> SendSMS(SMSMessage sms)
        {
			FirebaseAuthLink firebaseAuthLink = await getFirebaseAuthenticationLink(configuration);

            TwilioClient.Init(accountSid, authToken);
			sms.MediaUrl = await Helpers.FileHelper.UploadAttachmentsToFirebase(configuration, firebaseAuthLink);
            
			try
            {
				await SMS.Send(new NetworkCredential(accountSid, authToken, configuration["Settings:Tvilio:domain"]), sms);
            }
			finally
			{
				await Helpers.FileHelper.CleanAttachmentsFoldre(configuration, firebaseAuthLink);
			}

			return Ok();
        }

		[HttpGet]
		public async Task<IActionResult> GetAccounts()
		{
			await Task.Yield();
			return Ok(SMS.GetAccounts(new NetworkCredential(accountSid, authToken, configuration["Settings:Tvilio:domain"])));
        }

		
		private readonly string accountSid;
		private readonly string authToken;
		private readonly IConfiguration configuration;
		private async static Task<FirebaseAuthLink> getFirebaseAuthenticationLink(IConfiguration configuration) =>
			await new FirebaseAuthProvider(new FirebaseConfig(configuration["Settings:Firebase:apiKey"])).SignInWithEmailAndPasswordAsync(
				configuration["Settings:Firebase:authEmail"], configuration["Settings:Firebase:authPassword"]);

	}
}
