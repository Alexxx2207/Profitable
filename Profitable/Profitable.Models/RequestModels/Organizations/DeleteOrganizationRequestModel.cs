using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.RequestModels.Organizations
{
    public class DeleteOrganizationRequestModel
    {
        public Guid RequesterId { get; set; }

        public Guid OrganizationId { get; set; }
    }
}
