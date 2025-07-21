using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.Infrastructure.DatabaseContext;
using CitiesManager.Core.Models;

namespace CitiesManager.WebAPI.Controllers.v2
{
    [ApiVersion("2.0")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string?>>> GetCities()
        {
            var cities = await _context.Cities
             .OrderBy(temp => temp.CityName)
             .Select(temp => temp.CityName)
             .ToListAsync();
            return cities;
        }
    }
}
