using CitiesWeb.API.Data;
using CitiesWeb.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWeb.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private CitiesDbContext _context;

        public CitiesController(CitiesDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var cities = _context.Cities.ToList();
            return Ok(Json(cities));
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest(city);
            }
            return Ok(Json(city));
        }
        [HttpPost]
        public IActionResult CreateCity([FromBody] City city)
        {
            int idmax = _context.Cities.Max(c => c.Id);
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var newCity = new City()
            {
                Id = idmax++,
                Name = city.Name,
                Description = city.Description,
                PointOfInterests = city.PointOfInterests
            };
            return Ok(newCity);
        }

    }
}
