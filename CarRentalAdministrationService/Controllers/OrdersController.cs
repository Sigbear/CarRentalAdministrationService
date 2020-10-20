using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalAdministrationService.Model;
using CarRentalAdministrationService.Dto;

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
            var orders  = await _context.Orders.ToListAsync();
            var ordersFull = from order in orders
                             join category in _context.CarCategories
                             on order.CarCategory.CarCategoryId equals category.CarCategoryId
                             select order;
            if(ordersFull == null)
            {
                return NotFound();
            }
            return ordersFull.ToList();
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
        public async Task<ActionResult<OrderDto>> PostOrder(OrderDto orderDto)
        {
            var category = await _context.CarCategories.FirstAsync(category => category.Category.Equals(orderDto.CarCategory));
            
            if(category == null)
            {
                return NotFound($"Category {orderDto.CarCategory} could not be found in the system.");
            }
            var order = new Order
            {
                Created = orderDto.Created,
                CustomerDateOfBirth = orderDto.CustomerDateOfBirth,
                CarCategory = category
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        private bool CarCateGoryExists(string category)
        {
            return _context.CarCategories.Any(e => e.Category.Equals(category));
        }

    }
}
