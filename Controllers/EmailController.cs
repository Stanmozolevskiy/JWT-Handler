using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyLibraries;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace Utility.Controllers
{
    [Route("[controller]"), ApiController, AllowAnonymous]
    public class EmailController: ControllerBase
    {
        public EmailController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("saveAttachment")]
        public async Task<IActionResult> SaveAttachment(List<IFormFile> files)
        {
            foreach (IFormFile file in files)
                Helpers.FileHelper.CopyFileLocally(file.OpenReadStream(), 
                    $"{System.IO.Directory.CreateDirectory("Attachments").Name}/{file.FileName}");
            
           await Task.Yield();
           return Ok(); 
        }


        [HttpPost("rempveAttachment")]
        public async Task<IActionResult> RempveAttachment(IFormCollection fileNames)
        {

			Helpers.FileHelper.DeleteFileLocally(fileNames["fileNames"].Single());
			await Task.Yield();
			return Ok();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(EmailMessage email)
        {
            string[] recipience = null;
            string[] cc = null;

            if (!String.IsNullOrEmpty(email.To))
                recipience = email.To?.Split(",");
            
            if (!String.IsNullOrEmpty(email.Cc))
                    cc = email.Cc?.Split(",");

            try
            {
			await Email.Send(configuration["Settings:SendGrid:apiKey"], email.Subject, email.Message, Helpers.FileHelper.GetAttachments(), 
                configuration["Settings:SendGrid:originEmail"], null, recipience, cc, null);
			return Ok(Response);
            }
            catch(Exception e)
            {
                return Ok(e);
            }
            finally
            {
                Helpers.FileHelper.CleanAttachmentsFoldre();
            }
        }
        private IConfiguration configuration;
    }

    public class EmailMessage
    {
        public string To;
        public string Cc;
        public string Subject;
        public string Message;
    }
}
