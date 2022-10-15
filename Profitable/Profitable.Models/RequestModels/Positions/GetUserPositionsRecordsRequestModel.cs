﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.RequestModels.Positions
{
    public class GetUserPositionsRecordsRequestModel
    {
        public string UserEmail { get; set; }

        public int Page { get; set; }

        public int PageCount { get; set; }

        public string OrderPositionsRecordBy { get; set; }
    }
}
