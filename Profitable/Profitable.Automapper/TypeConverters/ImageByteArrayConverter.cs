using AutoMapper;
using Profitable.GlobalConstants;

namespace Profitable.Automapper.TypeConverters
{
    public class ImageByteArrayConverter : IValueConverter<string, byte[]>
    {
        private readonly ImageFor imageFor;

        public ImageByteArrayConverter(ImageFor imageFor)
        {
            this.imageFor = imageFor;
        }
        public byte[] Convert(string sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(sourceMember))
            {
                return Array.Empty<byte>();
            }

            try
            {
                var filePath =
                $"{GlobalServicesConstants.UploadsFolderPath}" +
                $"{GlobalServicesConstants.DirectorySeparatorChar}" +
                $"{imageFor}" +
                $"{GlobalServicesConstants.DirectorySeparatorChar}" +
                $"{sourceMember}";

                var fileBytes = File.ReadAllBytes(filePath);

                return fileBytes;
            }
            catch (Exception)
            {
                return Array.Empty<byte>();
            }
        }
    }
}
