﻿namespace Profitable.GlobalConstants
{
    public class GlobalServicesConstants
    {

        public static readonly int PostsCountInPage = 6;

        public static readonly string EntityDoesNotExist = "Entity does not exist!";

        public static readonly string UploadsFolderInProject = "Profitable.Web";

        public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;

        public static readonly string UploadsFolderPath =
            Path.GetDirectoryName(Directory.GetCurrentDirectory())
            + $"{DirectorySeparatorChar}{UploadsFolderInProject}{DirectorySeparatorChar}Uploads";
    }
}
