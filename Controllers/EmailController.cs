using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyLibraries;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using DataModels;

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
              await Helpers.FileHelper.CopyFileLocally(file.OpenReadStream(), 
                    $"{System.IO.Directory.CreateDirectory("Attachments").Name}/{file.FileName}");
            
           return Ok(); 
        }


        [HttpPost("rempveAttachment")]
        public async Task<IActionResult> RempveAttachment(IFormCollection fileNames)=>
             Ok(await Helpers.FileHelper.DeleteFileLocally(fileNames["fileNames"].Single()));
        

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(EmailMessage email)
        {
            string[] recipience = null;
            string[] cc = null;

            if (!string.IsNullOrEmpty(email.To))
                recipience = email.To?.Split(",");
            
            if (!string.IsNullOrEmpty(email.Cc))
                    cc = email.Cc?.Split(",");

            try
            {
                await Email.Send(configuration["Settings:SendGrid:apiKey"], email.Subject, email.Message, await Helpers.FileHelper.GetAttachments(), 
                configuration["Settings:SendGrid:originEmail"], null, recipience, cc, null);
			    return Ok(Response);
            }
            catch(Exception e)
            {
                return Ok(e);
            }
            finally
            {
                await Helpers.FileHelper.CleanAttachmentsFoldre();
            }
        }
        private IConfiguration configuration;
    }
}
