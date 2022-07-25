using AutoMapper;
using Profitable.GlobalConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(string.IsNullOrEmpty(sourceMember))
            {
                return Array.Empty<byte>();
            }

            var filePath = $"{GlobalServicesConstants.UploadsFolderPath}\\{imageFor}\\{sourceMember}";
            var fileBytes = File.ReadAllBytes(filePath);

            return fileBytes;
        }
    }
}
