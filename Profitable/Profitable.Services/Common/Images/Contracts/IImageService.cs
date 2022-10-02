using Profitable.GlobalConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Services.Common.Images.Contracts
{
	public interface IImageService
	{
		Task<string> SaveUploadedImageAsync(ImageFor imageFor, string fileName, string base64Image);

		Task<string> DeleteUploadedImageAsync(ImageFor imageFor, string fileName);
	}
}
