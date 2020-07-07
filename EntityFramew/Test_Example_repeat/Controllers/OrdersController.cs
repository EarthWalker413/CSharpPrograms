using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test_Example_repeat.DTOs;
using Test_Example_repeat.Model;

namespace Test_Example_repeat.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        public readonly OrderDbContext _context;

        public OrdersController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                var list = (from order in _context.Orders
                            join co in _context.ConfectionaryOrders on order.IdOrder equals co.IdOrder
                            join confectionary in _context.Confectionaries on co.IdConfectionary equals confectionary.IdConfectionary
                            join customer in _context.Customers on order.IdCustomer equals customer.IdCustomer
                            select new
                            {
                                DateAccepted = order.DateAccepted,
                                Customer = customer.FirstName,
                                Notes = order.Notes,
                                Quantity = co.Quantity,
                                Confectionary = confectionary.Name
                            }).ToList();
                return Ok(list);
            } catch (Exception e)
            {
                return BadRequest("Bad Request");
            }
        }

        [HttpGet("{clientName}")]
        public IActionResult GetOrders(string clientName)
        {
            try
            {
                var list = (from order in _context.Orders
                            join co in _context.ConfectionaryOrders on order.IdOrder equals co.IdOrder
                            join confectionary in _context.Confectionaries on co.IdConfectionary equals confectionary.IdConfectionary
                            join customer in _context.Customers on order.IdCustomer equals customer.IdCustomer
                            where customer.FirstName == clientName
                            select new
                            {
                                DateAccepted = order.DateAccepted,
                                Customer = customer.FirstName,
                                Notes = order.Notes,
                                Quantity = co.Quantity,
                                Confectionary = confectionary.Name
                            }).ToList();
                return Ok(list);
            } catch (Exception e)
            {
                return BadRequest("Bad request");
            }
        }

        [HttpPost("{IdCustomer}/orders")]
        public IActionResult AddOrder(int IdCustomer, AddOrderRequest request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {

                try
                {

                    if (!_context.Customers.Any(c => c.IdCustomer == IdCustomer))
                    {
                        return BadRequest("No such customer");
                    }

                    if (request.DateAccepted == null || request.Notes == null || request.Confectionaries == null)
                    {
                        return BadRequest("Not all data is valid");
                    }

                    _context.Orders.Add(new Order()
                    {
                        DateAccepted = request.DateAccepted,
                        Notes = request.Notes,
                        IdCustomer = IdCustomer
                    });

                    _context.SaveChanges();

                    var newOrderId = _context.Orders.Max(c => c.IdOrder);

                    foreach (ConfectionaryListRequest request1 in request.Confectionaries)
                    {

                        if (request1.Quantity < 0)
                        {
                            return BadRequest("Quantity cannot be negative");
                        }

                        if (_context.Confectionaries.Any(c => c.Name == request1.Name))
                        {
                            return BadRequest("There is already such confectionary");
                        }
                        else
                        {
                            _context.Confectionaries.Add(new Confectionary()
                            {
                                Name = request1.Name
                            });

                            var newConfectionaryId = _context.Confectionaries.Max(c => c.IdConfectionary);

                            _context.ConfectionaryOrders.Add(new ConfectionaryOrder()
                            {
                                IdConfectionary = newConfectionaryId,
                                IdOrder = newOrderId,
                                Quantity = request1.Quantity
                            });
                            _context.SaveChanges();

                        }
                    }

                    transaction.Commit();
                    return Ok("Order is Added");
                } catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest("BadRequest");
                }
            }
        }

        [HttpDelete("{IdOrder}")]
        public IActionResult DeleteOrder(int IdOrder)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                if(!_context.Orders.Any(o => o.IdOrder == IdOrder))
                {
                    return BadRequest("There s no such order");
                }
                var orderToRemove = _context.Orders.Where(o => o.IdOrder == IdOrder).First();

                _context.Orders.Remove(orderToRemove);
                _context.SaveChanges();

                transaction.Commit();
                return Ok("Order deleted");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateConfectionary(int id, Confectionary confectionary)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (!_context.Confectionaries.Any(c => c.IdConfectionary == id))
                    {
                        transaction.Rollback();
                        return BadRequest("No such element");
                    }

                    if (confectionary.Price < 0)
                    {
                        transaction.Rollback();
                        return BadRequest("Price cannot be less negative");
                    }

                    var oldConfectionary = _context.Confectionaries.Where(c => c.IdConfectionary == id).First();

                    if (confectionary.Name != null)
                    {
                        oldConfectionary.Name = confectionary.Name;
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest("There is no name");
                    }

                    if (confectionary.Price != oldConfectionary.Price)
                    {
                        oldConfectionary.Price = confectionary.Price;
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest("Price cannot be negative");
                    }

                    if (confectionary.Type != null)
                    {
                        oldConfectionary.Type = confectionary.Type;
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                    return Ok("Confectionary is updated");

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest("Bad Request");
                }
            }
        }
    }
}