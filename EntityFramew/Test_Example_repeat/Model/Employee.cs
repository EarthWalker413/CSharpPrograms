using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Example_repeat.Model
{
    public class Employee
    {
        public int IdEmployee { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
