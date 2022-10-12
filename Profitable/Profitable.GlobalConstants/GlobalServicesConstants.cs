using System.Text.RegularExpressions;

namespace Profitable.Common.GlobalConstants
{
    public class GlobalServicesConstants
    {

        public static readonly int PostsMaxCountInPage = 100;

        public static readonly short PostMaxLength = 2000;

        public static readonly short CommentMaxLength = 1000;

        public static readonly string EntityDoesNotExist = "Entity does not exist!";

        public static readonly string UploadsFolderInProject = "Profitable.Web";

        public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;

        public static readonly string UploadsFolderPath =
            Path.GetDirectoryName(Directory.GetCurrentDirectory())
            + $"{DirectorySeparatorChar}{UploadsFolderInProject}{DirectorySeparatorChar}Uploads";
	}
}
