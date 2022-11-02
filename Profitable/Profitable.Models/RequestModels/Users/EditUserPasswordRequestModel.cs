namespace Profitable.Models.RequestModels.Users
{
    public class EditUserPasswordRequestModel
    {
        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
