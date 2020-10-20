using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalAdministrationService.Model;

namespace CarRentalAdministrationService.Controllers
{
    [Route("api/carcategories")]
    [ApiController]
    public class CarCategoriesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public CarCategoriesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/carcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarCategory>>> GetCarCategories()
        {
            return await _context.CarCategories.ToListAsync();
        }

        // POST: api/carcategories
        [HttpPost]
        public async Task<ActionResult<CarCategory>> PostCarCategory(CarCategory carCategory)
        {
            _context.CarCategories.Add(carCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarCategory", new { id = carCategory.CarCategoryId }, carCategory);
        }

        private bool CarCategoryExists(int id)
        {
            return _context.CarCategories.Any(e => e.CarCategoryId == id);
        }
    }
}
