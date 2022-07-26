using Profitable.GlobalConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.InputModels.Users
{
    public class RegisterUserInputModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string? ProfilePictureURL { get; set; }

    }
}
