using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SEntidad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    public class EntidadController : Controller
    {
        
    }
}
