namespace Profitable.Services.Common.Images.Contracts
{
	using Profitable.Common.Enums;

	public interface IImageService
	{
		Task<string> SaveUploadedImageAsync(ImageFor imageFor, string fileName, string base64Image);

		Task<string> DeleteUploadedImageAsync(ImageFor imageFor, string fileName);
	}
}
