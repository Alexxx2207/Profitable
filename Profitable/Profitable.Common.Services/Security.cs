namespace Profitable.Common.Services
{
	using Konscious.Security.Cryptography;
	using Profitable.Common.GlobalConstants;
	using Profitable.Common.Models;
	using System;
	using System.Security.Cryptography;
	using System.Text;

	public static class Security
	{
		public static HashPasswordResult HashUserPassword(string rawPassword, Guid userId)
		{
			byte[] passwordUnhashed = Encoding.UTF8.GetBytes(rawPassword);
			byte[] salt = GenerateSalt();
			byte[] userUuidBytes = Encoding.UTF8.GetBytes(userId.ToString());

			var hash = GenerateHashedPassword(passwordUnhashed, salt, userUuidBytes);

			return new HashPasswordResult()
			{
				Salt = Convert.ToBase64String(salt),
				PasswordHash = Convert.ToBase64String(hash)
			};
		}

		public static bool VerifyPassword(string rawPassword, Guid userId, string userSalt, string hashedPassword)
		{
			byte[] passwordUnhashed = Encoding.UTF8.GetBytes(rawPassword);
			byte[] userUuidBytes = Encoding.UTF8.GetBytes(userId.ToString());
			byte[] salt = Convert.FromBase64String(userSalt);

			var hash = GenerateHashedPassword(passwordUnhashed, salt, userUuidBytes);

			var hashedPasswordAsBytes = Convert.FromBase64String(hashedPassword);

			return hash.SequenceEqual(hashedPasswordAsBytes);
		}

		private static byte[] GenerateHashedPassword(
			byte[] rawData,
			byte[] salt,
			byte[] userUuidBytes)
		{
			var argon2 = new Argon2id(rawData);

			argon2.DegreeOfParallelism = 16;
			argon2.MemorySize = 8192;
			argon2.Iterations = 40;
			argon2.Salt = salt;
			argon2.AssociatedData = userUuidBytes;

			return argon2.GetBytes(128);
		}

		private static byte[] GenerateSalt()
		{
			return RandomNumberGenerator.GetBytes(GlobalServicesConstants.SaltByteArraySize);
		}
	}
}
