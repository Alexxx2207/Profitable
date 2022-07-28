namespace Profitable.Models.RequestModels.Users
{
    public class RegisterUserRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string? ProfilePictureURL { get; set; }

    }
}
