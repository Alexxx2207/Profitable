using Profitable.Common.Enums;

namespace Profitable.Services.Common.Images.Contracts
{
	public interface IImageService
	{
		Task<string> SaveUploadedImageAsync(ImageFor imageFor, string fileName, string base64Image);

		Task<string> DeleteUploadedImageAsync(ImageFor imageFor, string fileName);
	}
}
