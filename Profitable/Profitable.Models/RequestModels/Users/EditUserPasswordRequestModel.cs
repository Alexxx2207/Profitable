namespace Profitable.Models.RequestModels.Users
{
    public class EditUserPasswordRequestModel
    {
        public string OldPassword { get; set; }

        public string newPassword { get; set; }
    }
}
