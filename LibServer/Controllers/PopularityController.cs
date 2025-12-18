using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediaModel;
using System.Security.Cryptography;

namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopularityController : ControllerBase
    {
        private static readonly string[] Adjectives = new[]
        {
            "Good Food", "Quiet", "Atmospheric", "Cozy", "Kind Staff"
        };
//        private static readonly string[] Stores = new[]
//{
//    "Books & Co", "PageTurner", "StoryHub", "BookNest", "ReadersPoint",
//    "NovelWorld", "PaperTrail", "ChapterHouse", "BoundPages", "InkSquare"
//};

        private readonly ILogger<PopularityController> _logger;

        public PopularityController(ILogger<PopularityController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetPopularity")]
        public IEnumerable<Popularity> Get()
        {
            return Enumerable.Range(1, 10).Select(index => new Popularity
            {

                Thoughts = Adjectives[Random.Shared.Next(Adjectives.Length)],
                PopularityChartCount = index,
                Store = new[] { "Books & Co", "PageTurner", "StoryHub", "BookNest", 
                    "ReadersPoint", "NovelWorld", "PaperTrail", 
                    "ChapterHouse", "BoundPages", "InkSquare" }[Random.Shared.Next(10)]

            })
            .ToArray();
        }
    }
}
