using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeometricProgramming.ConsumerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PersistInput()
        {
            //TODO :: Add inputs to database.
            return Ok();
        }
    }
}
