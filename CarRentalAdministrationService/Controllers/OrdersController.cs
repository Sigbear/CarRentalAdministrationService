using CarRentalAdministrationService.Dto;
using CarRentalAdministrationService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var orders = await _context.Orders.Include(order => order.Car).ToListAsync();
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
            }
            catch (InvalidOperationException)
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
                order = await _context.Orders
                        .Include(order => order.Car)
                        .ThenInclude(car => car.CarCategory)
                        .FirstAsync(order => order.OrderId == returnOrderDto.BookingNr
                                          && returnOrderDto.ReturnDate > order.Closed);
                if (order == null)
                {
                    return NotFound();
                }
                if (returnOrderDto.MileageInKm < order.Car.MileageInKilometers)
                {
                    return BadRequest("Stated mileage is lower than mileage at point of rental, please check that the information is correct and try again.");
                }
                bool returnTimeLessThanRentalTime = DateTime.Compare(returnOrderDto.ReturnDate, order.Created) < 0;
                if (returnTimeLessThanRentalTime)
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
                bool OrderNotFoundOrCostModelNotImplemented = (ex is InvalidOperationException || ex is NotImplementedException);
                if (OrderNotFoundOrCostModelNotImplemented)
                {
                    return NotFound();
                }
                else
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
            int deltaMileageKm = returnOrderDto.MileageInKm - order.Car.MileageInKilometers;
            double mileageCost = order.Car.CarCategory.KilometerPrice * deltaMileageKm;

            switch (order.Car.CarCategory.Category.ToLower())
            {
                case "compact":
                    return baseCost;
                case "premium":
                    return baseCost * 1.2 + mileageCost;
                case "minivan":
                    return baseCost * 1.7 + mileageCost;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
