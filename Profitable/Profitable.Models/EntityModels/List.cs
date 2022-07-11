using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Models.EntityModels
{
    public class List : EntityBase
    {
        public List()
        {
            GUID = Guid.NewGuid().ToString();
            FinancialInstruments = new HashSet<ListsFinancialInstruments>();
        }

        [Key]
        public string GUID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string TraderId { get; set; }
        public Trader Trader { get; set; }

        public ICollection<ListsFinancialInstruments> FinancialInstruments { get; set; }

    }
}
