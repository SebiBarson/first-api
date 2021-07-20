using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Controllers
{
    public class TestController: Controller
    {
        /// <summary>
        /// Ciueva
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/user")]
        public IActionResult Get()
        {
            return Ok(new { name = "Sebi" });
        }
    }
}
