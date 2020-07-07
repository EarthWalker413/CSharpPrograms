using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example_test.DTOs
{
    public class AddAnimalRequest
    {
        public int IdAnimal{get;set;}
        public string AnimalName { get; set; }

        public string AnimalType { get; set; }

        public DateTime AdmissionDate { get; set; }

        public int IdProcedure { get; set; }
        
        public string ProcedureName { get; set; }

        public string Description { get; set; }

        public int IdOwner { get; set; }
    }
}
