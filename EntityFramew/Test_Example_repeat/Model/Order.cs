using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Example_repeat.Model
{
    public class Order
    {
        public int IdOrder { get; set; }
        public DateTime DateAccepted { get; set; }
        public DateTime? DateFinished { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdEmployee { get; set; }
        public string Notes { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<ConfectionaryOrder> ConfectionaryOrders { get; set; }
    }
}
