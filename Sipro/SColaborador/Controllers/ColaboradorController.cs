using System;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace SColaborador.Controllers
{
    [Route("api/[controller]")]
    public class ColaboradorController : Controller
    {
        private class stcolaborador
        {
            public int id;
            public String pnombre;
            public String snombre;
            public String otrosNombres;
            public String ppellido;
            public String sapellido;
            public String otrosApellidos;
            public int ueunidadEjecutora;
            public String unidadejecutoranombre;
            public int entidad;
            public String entidadnombre;
            public int ejercicio;
            public long cui;
            public String usuario;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public String nombreCompleto;
        }

        [HttpPost]
        public IActionResult ColaboradoresPorPagina([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("1", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult Colaborador([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("2", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Colaborador(int id, [FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("3", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Colaborador(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("4", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("5", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{usuario}")]
        public IActionResult ValidarUsuario(string usuario)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("6", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ColaboradorPorId(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("7", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }
    }
}
