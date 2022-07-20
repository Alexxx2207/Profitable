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
        public byte[] Convert(string sourceMember, ResolutionContext context)
        {
            var filePath = GlobalServicesConstants.UploadsFolderPath + sourceMember;
            var fileBytes = File.ReadAllBytes(filePath);

            return fileBytes;
        }
    }
}
