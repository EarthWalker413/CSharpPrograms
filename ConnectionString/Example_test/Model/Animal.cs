using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example_test.Model
{
    public class Animal
    {

        public int IdAnimal { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public DateTime AdmissionDate { get; set; }

        public int IdOwner { get; set; }

    }
}
