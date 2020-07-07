using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Example_repeat.Model
{
    public class ConfectionaryOrder
    {
        public int IdOrder { get; set; }
        public int IdConfectionary { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public virtual Order Order { get; set; }
        public virtual Confectionary Confectionary {get;set;}
    }
}
