using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        // POST /<controller>
        [HttpPost]
        public IActionResult In([FromBody]dynamic data)
        {
            return Ok("Datos: " + data.user + ", " + data.password);
            //return null;
        }

        [HttpPost]
        public IActionResult Out()
        {
            return Ok("Sign out");
            //return null;
        }
    }
}
