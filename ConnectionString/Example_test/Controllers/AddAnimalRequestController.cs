using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Example_test.DTOs;
using Example_test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example_test.Controllers
{
    [ApiController]
    [Route("api/addAnimal")]
    public class AddAnimalRequestController : ControllerBase
    {
        private string _connString = "Data Source=(localdb)\\ProjectsV13;Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private IAnimalAddingDbService _service;

        public AddAnimalRequestController(IAnimalAddingDbService dbService)
        {
            _service = dbService;
        }

        [HttpPost]
        public IActionResult AddAnimal(AddAnimalRequest request)
        {
            if (request.AnimalName == null || request.AnimalType == null || request.AdmissionDate == null || request.IdOwner == 0 || request.ProcedureName == null || request.Description == null)
            {
                return BadRequest("Not Valid");
            }

            using (SqlConnection con = new SqlConnection(_connString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                try
                {

                    com.Parameters.AddWithValue("AnimalName", request.AnimalName);
                    com.Parameters.AddWithValue("AnimalType", request.AnimalType);
                    com.Parameters.AddWithValue("AdmissionDate", request.AdmissionDate);
                    com.Parameters.AddWithValue("ProcedureName", request.ProcedureName);
                    com.Parameters.AddWithValue("Description", request.Description);
                    com.Parameters.AddWithValue("OwnerId", request.IdOwner);






                    if (request.IdAnimal == 0)
                    {
                        var newAnimalId = new Random().Next();

                        com.Parameters.AddWithValue("NewAnimalId", newAnimalId);

                        com.CommandText = " SET IDENTITY_INSERT Animal On insert into Animal(IdAnimal, Name, Type, AdmissionDate, IdOwner) values(@NewAnimalId, @AnimalName, @AnimalType, @AdmissionDate, @OwnerId)";

                        com.ExecuteNonQuery();
                    }
                    else
                    {
                        com.Parameters.AddWithValue("AnimalId", request.IdAnimal);
                    }

                    if (request.IdProcedure == 0)
                    {
                        var newProcedureId = new Random().Next();

                        com.Parameters.AddWithValue("NewProcedureId", newProcedureId);

                        com.CommandText = "SET IDENTITY_INSERT AProcedure On insert into AProcedure(IdProcedure, Name, Description) values(@NewProcedureId, @ProcedureName, @Description)";
                        com.ExecuteNonQuery();
                    }
                    else
                    {
                        com.Parameters.AddWithValue("ProcedureId", request.IdProcedure);
                    }

                    if (request.IdProcedure == 0 && request.IdAnimal != 0)
                    {
                        var date = DateTime.Now;
                        com.Parameters.AddWithValue("Date", date);
                        com.CommandText = "insert into Procedure_Animal(Procedure_IdProcedure, Animal_IdAnimal, Date) values (@NewProcedureId,@AnimalId,  @Date)";

                        com.ExecuteNonQuery();
                    }
                    else if (request.IdAnimal == 0 && request.IdProcedure != 0)
                    {
                        var date = DateTime.Now;
                        com.Parameters.AddWithValue("Date", date);
                        com.CommandText = "insert into Procedure_Animal(Procedure_IdProcedure, Animal_IdAnimal, Date) values (@ProcedureId,@NewAnimalId,  @Date)";

                        com.ExecuteNonQuery();
                    }
                    else if (request.IdAnimal == 0 && request.IdProcedure == 0)
                    {
                        var date = DateTime.Now;
                        com.Parameters.AddWithValue("Date", date);
                        com.CommandText = "insert into Procedure_Animal(Procedure_IdProcedure, Animal_IdAnimal, Date) values (@NewProcedureId,@NewAnimalId,  @Date)";

                        com.ExecuteNonQuery();
                    }


            }
                catch (Exception e)
            {

                tran.Rollback();
                return BadRequest("Something is wrong");
            }
            tran.Commit();
            }
            return Ok(request);

            
        }
  
    }
}