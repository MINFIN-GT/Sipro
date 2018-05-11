using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using SiproModelCore.Models;
using SiproDAO.Dao;

namespace Sipro.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]/{codigo?}")]
    public class CooperanteController : Controller
    {
        private class stcooperante
        {
            public int codigo;
            public String nombre;
            public String siglas;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpGet]
        public IActionResult mensaje()
        {
            return Ok("hola");
        }

        // GET api/Cooperante/Cooperantes
        [HttpGet]
        [Authorize("Cooperantes - Visualizar")]
        public IActionResult Cooperantes()
        {
            List<Cooperante> cooperantes = CooperanteDAO.getCooperantes();
            List<stcooperante> stcooperantes = new List<stcooperante>();
            foreach (Cooperante cooperante in cooperantes)
            {
                stcooperante temp = new stcooperante();
                temp.codigo = cooperante.codigo;
                temp.descripcion = cooperante.descripcion;
                temp.estado = cooperante.estado;
                temp.fechaActualizacion = cooperante.fechaActualizacion != null ? cooperante.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : "";
                temp.fechaCreacion = cooperante.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                temp.nombre = cooperante.nombre;
                temp.usuarioActualizo = cooperante.usuarioActualizo;
                temp.usuarioCreo = cooperante.usuarioCreo;
                stcooperantes.Add(temp);
            }

            string response_text = JsonConvert.SerializeObject(stcooperantes);
            response_text = String.Join("", "\"cooperantes\":", response_text);
            response_text = String.Join("", "{\"success\":true,", response_text, "}");
            return Ok(response_text);
        }

        // POST api/Cooperante/Cooperantes
        [HttpPost]
        //[Authorize(Policy = "Cooperantes - Crear")]
        public IActionResult Cooperantes([FromBody]dynamic value)
        {
            bool esnuevo = (bool)value.esnuevo;
            Cooperante cooperante;

            if (esnuevo)
            {
                cooperante = new Cooperante();
                cooperante.codigo = (int)value.codigo;
                cooperante.descripcion = (string)value.descripcion;
                cooperante.ejercicio = (int)value.ejercicio;
                cooperante.estado = (int)value.estado;
                cooperante.fechaCreacion = DateTime.Now;
                cooperante.nombre = (string)value.nombre;
                cooperante.siglas = (string)value.siglas;
                cooperante.usuarioCreo = User.Identity.Name;
            }
            else
            {
                cooperante = CooperanteDAO.getCooperantePorCodigo((int)value.codigo);
                cooperante.codigo = (int)value.codigo;
                cooperante.descripcion = (string)value.descripcion;
                cooperante.ejercicio = (int)value.ejercicio;
                cooperante.estado = (int)value.estado;
                cooperante.fechaActualizacion = DateTime.Now;
                cooperante.nombre = (string)value.nombre;
                cooperante.siglas = (string)value.siglas;
                cooperante.usuarioActualizo = User.Identity.Name;
            }

            bool result = CooperanteDAO.guardarCooperante(cooperante);

            string response_text = String.Join("", "{ \"success\": ", (result ? true : false), ", "
                , "\"id\": " + cooperante.codigo, ","
                , "\"usuarioCreo\": \"", cooperante.usuarioCreo, "\","
                , "\"fechaCreacion\":\" ", cooperante.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"), "\","
                , "\"usuarioactualizo\": \"", cooperante.usuarioActualizo != null ? cooperante.usuarioActualizo : "", "\","
                , "\"fechaactualizacion\": \"", cooperante.fechaActualizacion != null ? cooperante.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : "", "\"" +
                " }");

            return Ok(response_text);
        }

        // POST api/Cooperante/Cooperantes/codigo
        [HttpDelete]
        //[Authorize(Policy = "Cooperantes - Eliminar")]
        public IActionResult Cooperantes(int codigo)
        {
            string response_text = "";
            if (codigo > 0)
            {
                Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo(codigo);
                cooperante.usuarioActualizo = User.Identity.Name;
                response_text = String.Join("", "{ \"success\": ", (CooperanteDAO.eliminarCooperante(cooperante) ? true : false), " }");
            }
            else
                response_text = "{ \"success\": false }";

            return Ok(response_text);
        }

        // POST api/Cooperante/CooperantesPagina
        [HttpPost]
        //[Authorize(Policy = "Cooperantes - Visualizar")]
        public IActionResult CooperantesPagina([FromBody]dynamic value)
        {
            List<Cooperante> cooperantes = CooperanteDAO.getCooperantesPagina((int)value.pagina, (int)value.numerocooperantes, (string)value.filtro_codigo,
                (string)value.filtro_nombre, (string)value.filtro_usuario_creo, (string)value.filtro_fecha_creacion, (string)value.columna_ordenada,
                (string)value.orden_direccion);

            List<stcooperante> stcooperantes = new List<stcooperante>();
            foreach (Cooperante cooperante in cooperantes)
            {
                stcooperante temp = new stcooperante();
                temp.codigo = cooperante.codigo;
                temp.descripcion = cooperante.descripcion;
                temp.estado = cooperante.estado;
                temp.fechaActualizacion = cooperante.fechaActualizacion != null ? cooperante.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : "";
                temp.fechaCreacion = cooperante.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                temp.nombre = cooperante.nombre;
                temp.siglas = cooperante.siglas;
                temp.usuarioActualizo = cooperante.usuarioActualizo;
                temp.usuarioCreo = cooperante.usuarioCreo;
                stcooperantes.Add(temp);
            }

            string response_text = JsonConvert.SerializeObject(stcooperantes);
            response_text = String.Join("", "\"cooperantes\":", response_text);
            response_text = String.Join("", "{\"success\":true,", response_text, "}");
            return Ok(response_text);
        }

        // POST api/Cooperante/TotalCooperantes
        [HttpPost]
        //[Authorize(Policy = "Cooperantes - Visualizar")]
        public IActionResult TotalCooperantes([FromBody]dynamic value)
        {
            string response_text = String.Join("", "{ \"success\": true, \"totalcooperantes\":", CooperanteDAO.getTotalCooperantes((string)value.filtro_codigo,
                (string)value.filtro_nombre, (string)value.filtro_usuario_creo, (string)value.filtro_fecha_creacion), " }");

            return Ok(response_text);
        }
    }
}