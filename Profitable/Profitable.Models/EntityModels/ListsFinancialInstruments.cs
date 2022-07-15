using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.EntityModels
{
    public class ListsFinancialInstruments : EntityBase
    {
        [Required]
        public string ListId { get; set; }
        public List List { get; set; }

        [Required]
        public string FinancialInstrumentId { get; set; }
        public FinancialInstrument FinancialInstrument { get; set; }

    }
}
