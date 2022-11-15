using AutoMapper;
using Profitable.Data.Seeding.Seeders;
using Profitable.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Common.Automapper
{
    public class COTMapper : Profile
    {
        public COTMapper()
        {
            CreateMap<ScrapeModel, COTReport>();
        }
    }
}
