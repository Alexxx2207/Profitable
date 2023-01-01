namespace Profitable.Common.GlobalConstants
{
	public static class GlobalServicesConstants
	{
		public static readonly int SaltByteArraySize = 64;

		public static readonly int PasswordMinLength = 6;

		public static readonly string UploadsFolderInProject = "Profitable.Web";

		public static readonly string InternalServerErrorMessage = "Internal Server Error";

		public static readonly string RequesterNotOwnerMessage = "Requester not owner of the entity";

		public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;

		public static readonly string UploadsFolderPath =
			Path.GetDirectoryName(Directory.GetCurrentDirectory())
			+ $"{DirectorySeparatorChar}{UploadsFolderInProject}{DirectorySeparatorChar}Uploads";

		public static string EntityDoesNotExist(string entity) => $"{entity} was not found!";

		public static readonly int WeeksOfCOTReportsForFirstSeed = 10;
	}
}
