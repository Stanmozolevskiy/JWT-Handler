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
		 public static void DeleteFileLocally(string fileName)
        {
            try
            {
			if (File.Exists(Path.Combine("Attachments", fileName)))
				File.Delete(Path.Combine("Attachments", fileName));
            }
            catch
            {
                throw;
            }
		}
		public static void CopyFileLocally(Stream stream, string destPath)
		{
			using (FileStream fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
				stream.CopyTo(fileStream);
		}

		public static IEnumerable<Tuple<MemoryStream, string>> GetAttachments()
        {
			List<Tuple<MemoryStream, string>> attachments = new List<Tuple<MemoryStream, string>>();
            try {
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
			}
            catch(Exception e)
            {
				Console.WriteLine(e);
            }

			return attachments;
		}

		public async static Task<List<Uri>> UploadAttachmentsToFirebase(IConfiguration configuration, FirebaseAuthLink firebaseAuthLink)
		{
			List<Uri> links = new List<Uri>();
			try
			{	
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
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			return links;
		}

		public static void CleanAttachmentsFoldre(IConfiguration configuration = null, FirebaseAuthLink firebaseAuthLink = null)
        {
            try
            {
				foreach (FileInfo file in new DirectoryInfo(@"Attachments").GetFiles())
				{
					string fileName = file.Name;
					string filePath = $"Attachments/{fileName}";
						File.Delete(filePath);

					if(configuration is not null && firebaseAuthLink is not null)
					cleanFilesFromFireStorage(fileName, configuration, firebaseAuthLink);
				}
            }
            catch(Exception e)
            {
				Console.WriteLine(e);
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
