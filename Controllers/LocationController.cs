using Microsoft.AspNetCore.Mvc;
using servicesharing.Data;
using System;
using System.Linq;

namespace servicesharing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLocations()
        {
            var locations = _context.Locations.Select(l => new
            {
                l.Name,
                l.Address,
                l.Latitude,
                l.Longitude,
                l.Details
            }).ToList();

            return Ok(locations);
        }
        [HttpGet("search")]
        public IActionResult SearchLocations(double latitude, double longitude, double radiusKm, string city = "")
        {
            var locations = _context.Locations
                .Where(l => (string.IsNullOrEmpty(city) || l.Address.Contains(city)) &&
                            GetDistance(latitude, longitude, l.Latitude, l.Longitude) <= radiusKm)
                .Select(l => new
                {
                    l.Name,
                    l.Address,
                    l.Latitude,
                    l.Longitude,
                    l.Details
                })
                .ToList();

            return Ok(locations);
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Радиус на Земята в км
            double dLat = DegreeToRadian(lat2 - lat1);
            double dLon = DegreeToRadian(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }
    }
}
