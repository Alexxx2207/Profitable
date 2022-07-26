namespace Profitable.Models.ViewModels.Users
{
    public class UserDetailsViewModel
    {
        public string GUID { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageType { get; set; }

        public byte[] ProfileImage { get; set; }
    }
}
