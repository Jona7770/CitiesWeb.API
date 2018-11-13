using AutoMapper;
using CitiesWeb.API.Data;
using CitiesWeb.API.Entities;
using CitiesWeb.API.Models;
using CitiesWeb.API.Services;
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
        private ICitiesRepository _repository;

        public CitiesController(ICitiesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IActionResult GetCities(bool includePointsOfInterest = false)
        {
            var cityEntities = _repository.GetCities(includePointsOfInterest);

            if (includePointsOfInterest)
            {
                var citiesResult = Mapper.Map<List<CityDTO>>(cityEntities);
                return Ok(citiesResult);
            }
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _repository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDTO>(city);
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }

        [HttpPost]
        public IActionResult CreateCity([FromBody]CityDTO city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var cityToReturn = Mapper.Map<City>(city);
            _repository.CreateCity(cityToReturn);
            return Ok(city);
        }

        [HttpPatch]
        public IActionResult PatchCity([FromBody]City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var cityToReturn = Mapper.Map<City>(city);
            _repository.PatchCity(cityToReturn);
            return Ok(city);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id, bool includePointsOfInterest)
        {
            var cityToUpdate = _repository.GetCity(id, includePointsOfInterest);
            if (cityToUpdate == null)
            {
                return NotFound();
            }

            _repository.UpdateCity(cityToUpdate);
            return NoContent();

        }

        [HttpDelete]
        public IActionResult DeleteCity(int id, bool includePointsOfInterest = true)
        {
            var cityToDelete = _repository.GetCity(id, includePointsOfInterest);
            if (cityToDelete == null)
            {
                return NotFound();
            }

            _repository.DeleteCity(cityToDelete);
            return Ok(cityToDelete);
        }
    }
}
