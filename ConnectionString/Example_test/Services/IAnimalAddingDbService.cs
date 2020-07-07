using Example_test.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example_test.Services
{
    public interface IAnimalAddingDbService
    {
        IActionResult AddAnimal(AddAnimalRequest request);
    }
}
