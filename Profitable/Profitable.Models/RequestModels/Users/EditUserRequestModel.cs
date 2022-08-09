namespace Profitable.Models.RequestModels.Users
{
    public class EditUserRequestModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Description { get; set; }
    }
}
