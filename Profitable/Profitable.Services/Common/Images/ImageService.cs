using Profitable.Common.Enums;
using Profitable.Common.GlobalConstants;
using Profitable.Services.Common.Images.Contracts;
using System.Text.RegularExpressions;

namespace Profitable.Services.Common.Images
{
	public class ImageService : IImageService
	{
		public async Task<string> SaveUploadedImageAsync(ImageFor imageFor, string fileName, string base64Image)
		{
			string time = Regex.Replace(DateTime.UtcNow.ToString(), @"\/|\:|\s", "");
			string newFileName = time + fileName;

			string path = GlobalServicesConstants.UploadsFolderPath +
				GlobalServicesConstants.DirectorySeparatorChar +
				imageFor.ToString() +
				GlobalServicesConstants.DirectorySeparatorChar +
				newFileName;

			await File.WriteAllBytesAsync(path, Convert.FromBase64String(base64Image));

			return newFileName;
		}

		public async Task<string> DeleteUploadedImageAsync(ImageFor imageFor, string fileName)
		{
			string path = GlobalServicesConstants.UploadsFolderPath +
				GlobalServicesConstants.DirectorySeparatorChar +
				imageFor.ToString() +
				GlobalServicesConstants.DirectorySeparatorChar +
				fileName;

			if (File.Exists(path))
			{
				File.Delete(path);
			}

			return fileName;
		}
	}
}
