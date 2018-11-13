using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using CitiesWeb.API.Data;
using CitiesWeb.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace CitiesWeb.API.Services
{
    public class CitiesRepository : ICitiesRepository
    {
        private CitiesDbContext _context;

        public CitiesRepository(CitiesDbContext context)
        {
            _context = context;
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void CreateCity(City city)
        {
            _context.Cities.Add(city);
            Save();
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
        }

        public IEnumerable<City> GetCities(bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest);
            }
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterests
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointOfInterests
                .Where(p => p.CityId == cityId).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void PatchCity(City city)
        {
            _context.Cities.Attach(city);
            EntityEntry<City> entry = _context.Entry(city);
            entry.State = EntityState.Modified;
            Save();
        }

        public void DeleteCity(City city)
        {
            _context.Cities.Remove(city);
            Save();
        }

        public void UpdateCity(City city)
        {
            _context.Cities.Update(city);
            Save();
        }
    }
}
