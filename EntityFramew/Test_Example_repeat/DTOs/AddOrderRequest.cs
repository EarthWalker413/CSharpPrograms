using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Example_repeat.Model;

namespace Test_Example_repeat.DTOs
{
    public class AddOrderRequest
    {
        public DateTime DateAccepted { get; set; }
        public string Notes { get; set; }
        public List<ConfectionaryListRequest> Confectionaries { get; set; }

    }
}
