using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiproModel.Models;
using Sipro.Dao;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sipro.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class PermisoController : Controller
    {
        

        // POST api/values
        [HttpPost]
        public IActionResult getPermisos([FromBody]string value)
        {
            List<Permiso> permisos = PermisoDAO.getPermisos();
            return Ok(JsonConvert.SerializeObject(permisos));
        }


    }
}
