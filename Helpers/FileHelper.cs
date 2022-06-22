using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;

namespace Utility.Helpers
{
    public static class FileHelper
	{
		public async static Task<bool> DeleteFileLocally(string fileName)
        {
			await Task.Yield();
			try
            {
				if (File.Exists(Path.Combine("Attachments", fileName)))
					File.Delete(Path.Combine("Attachments", fileName));
				return true;
            }
            catch
            {
                throw;
            }
		}
		public async static Task CopyFileLocally(Stream stream, string destPath)
		{
			await Task.Yield();
			using (FileStream fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
				stream.CopyTo(fileStream);
		}

		public async static Task<IEnumerable<Tuple<MemoryStream, string>>> GetAttachments()
        {
			await Task.Yield();
			List<Tuple<MemoryStream, string>> attachments = new List<Tuple<MemoryStream, string>>();
			foreach (FileInfo file in new DirectoryInfo(@"Attachments").GetFiles())
			{
				string fileName = file.Name;
				using (FileStream fs = File.OpenRead($"Attachments/{fileName}"))
				{
					using (MemoryStream ms = new MemoryStream())
					{
						fs.CopyTo(ms);
						attachments.Add(Tuple.Create(ms, fileName));
					}
				}    
			}        
			return attachments;
		}

		public async static Task<List<Uri>> UploadAttachmentsToFirebase(IConfiguration configuration, FirebaseAuthLink firebaseAuthLink)
		{
			List<Uri> links = new List<Uri>();
			foreach (FileInfo file in new DirectoryInfo(@"Attachments").GetFiles())
			{
				string fileName = file.Name;
				using (MemoryStream ms = new MemoryStream(File.ReadAllBytes($"Attachments/{fileName}")))
				{
					FirebaseStorageTask task = new FirebaseStorage(configuration["Settings:Firebase:bucket"], new FirebaseStorageOptions
					{
						AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
					})
						.Child("sms-attachments")
						.Child(fileName)
						.PutAsync(ms);

					links.Add(new Uri(await task));
				}
			}

			return links;
		}

		public async static Task CleanAttachmentsFoldre(IConfiguration configuration = null, FirebaseAuthLink firebaseAuthLink = null)
        {
			await Task.Yield();
			foreach (FileInfo file in new DirectoryInfo(@"Attachments").GetFiles())
			{
				string fileName = file.Name;
				File.Delete($"Attachments/{fileName}");
				if(configuration is not null && firebaseAuthLink is not null)
					cleanFilesFromFireStorage(fileName, configuration, firebaseAuthLink);
			}
		}

		private static void cleanFilesFromFireStorage(string fileName, IConfiguration configuration, FirebaseAuthLink firebaseAuthLink)
		{
			new FirebaseStorage(configuration["Settings:Firebase:bucket"], new FirebaseStorageOptions
			{
				AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
			})
				.Child("sms-attachments")
				.Child(fileName)
				.DeleteAsync();
		}		
	}
}
