using Example_test.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example_test.Services
{
    public interface IAnimalDbService
    {
        IActionResult GetAnimals(string sortBy);
    }
}
