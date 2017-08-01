using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorld.Model
{
    public class RentHouse
    {
        [DisplayName("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DisplayName("编号")]
        public string HomeCode { get; set; }

        [DisplayName("标题")]
        public string HomeTitle { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("价格")]
        public decimal Price { get; set; }

        [DisplayName("楼层")]
        public int Floor { get; set; }

        [DisplayName("描述")]
        public string Text { get; set; }
    }
}
