using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediaModel;

namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopularityController : ControllerBase
    {
        private static readonly string[] Adjectives = new[]
        {
            "riveting", "intense", "stressful", "cozy", "thiniking"
        };

        private readonly ILogger<PopularityController> _logger;

        public PopularityController(ILogger<PopularityController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetPopularity")]
        public IEnumerable<Popularity> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Popularity
            {

                Thoughts = Adjectives[Random.Shared.Next(Adjectives.Length)],
                PopularityChartCount = + 1,
                MyNumber = 38+18
            })
            .ToArray();
        }
    }
}
