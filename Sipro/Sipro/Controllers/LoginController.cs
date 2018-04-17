using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        // POST /<controller>
        [HttpPost()]
        public string Post([FromBody]dynamic data)
        {
            return "Datos: " + data.user + ", " + data.password;
            //return null;
        }
    }
}
