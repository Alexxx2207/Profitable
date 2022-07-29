namespace Profitable.Models.ResponseModels.Users
{
    public class UserDetailsResponseModel
    {
        public string Guid { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageType { get; set; }

        public byte[] ProfileImage { get; set; }
    }
}
