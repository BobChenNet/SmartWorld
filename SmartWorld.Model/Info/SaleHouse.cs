using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorld.Model
{
   public class SaleHouse
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string HomeCode { get; set; }

        public string HomeTitle { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public int Floor { get; set; }

        public string Text { get; set; }
    }
}
