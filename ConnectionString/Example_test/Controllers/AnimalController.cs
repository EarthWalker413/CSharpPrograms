
using Example_test.DTOs;
using Example_test.Model;
using Example_test.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Example_test.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private string _connString = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private IAnimalDbService _service;

        public AnimalController(IAnimalDbService dbService)
        {
            _service = dbService;
        }

        [HttpGet]
        public IActionResult GetAnimals(string sortBy)
        {
            try
            {
                var result = new List<Owner>();

                using (SqlConnection con = new SqlConnection(_connString))
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;

                    if (sortBy == null)
                    {
                        sortBy = "AdmissionDate";
                    }
                    var conCommand = "select Animal.Name,Animal.Type, Animal.AdmissionDate, Owner.LastName from Animal, Owner where Owner.IdOwner = Animal.IdOwner ORDER BY Animal." + sortBy + " desc";


                    Console.WriteLine(conCommand);
                    com.CommandText = conCommand;
                    //com.CommandText = "select Animal.Name,Animal.Type, Animal.AdmissionDate, Owner.LastName from Animal, Owner where Owner.IdOwner = Animal.IdOwner ORDER BY Animal.Name desc";
                    con.Open();

                    SqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var anOw = new Owner();

                        anOw.Name = dr["Name"].ToString();
                        anOw.Type = dr["Type"].ToString();
                        anOw.AdmissionDate = DateTime.Parse(dr["AdmissionDate"].ToString());
                        anOw.OwnerLastName = dr["LastName"].ToString();
                        result.Add(anOw);
                    }
                    dr.Close();

                    return Ok(result);
                }

            }
            catch (Exception e)
            {
                return BadRequest("Uncorrect table");
            }
            
            
            
        }

        

       
    }
}