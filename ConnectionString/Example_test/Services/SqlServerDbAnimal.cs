using Example_test.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Example_test.Services
{
    public class SqlServerDbAnimal : ControllerBase, IAnimalDbService

    {
        private string ConnString = "Data Source=db-mssql;Initial Catalog=s17159;Integrated Security=True";

        public IActionResult GetAnimals(string sortBy)
        {
           try
            {
                var result = new List<Owner>();

                using (SqlConnection con = new SqlConnection(ConnString))
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
                return BadRequest("Bad request"); 
            }
        }
    }
}
