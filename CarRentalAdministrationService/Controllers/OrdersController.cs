using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalAdministrationService.Model;
using CarRentalAdministrationService.Dto;
using System;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CarRentalAdministrationService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public OrdersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders  = await _context.Orders.Include(order => order.Car).ToListAsync();
            return orders;
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<CreateOrderDto>> PostOrder(CreateOrderDto orderDto)
        {
            Car car;
            try
            {
                car = await _context.Cars
                .Include(car => car.CarCategory)
                .FirstAsync<Car>(car => car.CarCategory.Category.Equals(orderDto.CarCategory) && car.Available);
            } catch (InvalidOperationException)
            {
                return NotFound("No cars available in that category");
            }

            // available car found with the right category
            if (car == null)
            {
                return NotFound($"Category {orderDto.CarCategory} could not be found in the system.");
            }
            var order = new Order
            {
                Created = orderDto.Created,
                CustomerDateOfBirth = orderDto.CustomerDateOfBirth,
                Car = car
            };
            car.Available = false;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);

        }

        // Patch: api/orders
        [HttpPatch]
        public async Task<ActionResult<ReturnOrderDto>> PostOrder(ReturnOrderDto returnOrderDto)
        {
            Order order;
            try
            {
                order = await _context.Orders.Include(order => order.Car).ThenInclude(car => car.CarCategory).FirstAsync(order => order.OrderId == returnOrderDto.BookingNr && !order.Car.Available);
                if (order == null)
                {
                    return NotFound();
                }
                // check that mileage is not less than it was when hired
                if (order.Car.MileageInKilometers >= returnOrderDto.MileageInKm)
                {
                    return BadRequest("Stated mileage is lower than mileage at point of rental, please check that the information is correct and try again.");
                }
                // check that the date is not earlier than point of hire.
                if (DateTime.Compare(returnOrderDto.ReturnDate, order.Created) < 0)
                {
                    return BadRequest("Return time is earlier than pick up time. Please verify return time and try again.");
                }

                double cost = CalculateCost(order, returnOrderDto);
                var receipt = new OrderReceiptDto { Cost = cost };

                order.Car.MileageInKilometers = returnOrderDto.MileageInKm;
                order.Car.Available = true;
                order.Closed = returnOrderDto.ReturnDate;
                await _context.SaveChangesAsync();
                
                return Ok(receipt);

            }
            catch (Exception ex)
            {
                if(ex is InvalidOperationException || ex is NotImplementedException)
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }
        }

        private double CalculateCost(Order order, ReturnOrderDto returnOrderDto)
        {

            int baseDayRentalCost = order.Car.CarCategory.BaseDayRentalCost;
            double numberOfDays = (returnOrderDto.ReturnDate - order.Created).TotalDays;
            double baseCost = baseDayRentalCost * numberOfDays;
            double kmPrice = order.Car.CarCategory.KilometerPrice * (returnOrderDto.MileageInKm - order.Car.MileageInKilometers);

            switch(order.Car.CarCategory.Category.ToLower())
            {
                case "compact":
                    return baseCost;
                case "premium":
                    return baseCost * 1.2 + kmPrice;
                case "minivan":
                    return baseCost * 1.7 + kmPrice;
                default:
                    throw new NotImplementedException();
            }
        }       
    }
}
