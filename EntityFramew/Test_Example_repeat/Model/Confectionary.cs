using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Example_repeat.Model
{
    public class Confectionary
    {
        public int IdConfectionary { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ConfectionaryOrder> ConfectionaryOrders { get; set; }
    }
}
