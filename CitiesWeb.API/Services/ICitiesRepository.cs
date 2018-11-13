using CitiesWeb.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWeb.API.Services
{
    public interface ICitiesRepository
    {
        bool CityExists(int cityId);
        IEnumerable<City> GetCities(bool includePointsOfInterest);
        City GetCity(int cityId, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        void CreateCity(City city);
        void PatchCity(City city);
        void UpdateCity(City city);
        void DeleteCity(City city);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        bool Save();

    }
}
