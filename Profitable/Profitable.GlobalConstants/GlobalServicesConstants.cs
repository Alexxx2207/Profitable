using System.Text.RegularExpressions;

namespace Profitable.GlobalConstants
{
    public class GlobalServicesConstants
    {

        public static readonly int PostsMaxCountInPage = 100;

        public static readonly string EntityDoesNotExist = "Entity does not exist!";

        public static readonly string UploadsFolderInProject = "Profitable.Web";

        public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;

        public static readonly string UploadsFolderPath =
            Path.GetDirectoryName(Directory.GetCurrentDirectory())
            + $"{DirectorySeparatorChar}{UploadsFolderInProject}{DirectorySeparatorChar}Uploads";

        public static async Task<string> SaveUploadedImageAsync(ImageFor imageFor, string fileName, string base64Image)
        {
            string time = Regex.Replace(DateTime.Today.ToString(), @"\/|\:|\s", "");
            string newFileName = time + fileName;

            string path = GlobalServicesConstants.UploadsFolderPath +
                GlobalServicesConstants.DirectorySeparatorChar +
                imageFor.ToString() +
                GlobalServicesConstants.DirectorySeparatorChar +
                newFileName;

            await File.WriteAllBytesAsync(path, Convert.FromBase64String(base64Image));

            return newFileName;
        }
    }
}
