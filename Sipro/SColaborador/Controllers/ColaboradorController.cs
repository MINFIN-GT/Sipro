using System;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using SiproModelCore.Models;
using System.Collections.Generic;
using SiproDAO.Dao;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace SColaborador.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ColaboradorController : Controller
    {
        private class stcolaborador
        {
            public int id;
            public String pnombre;
            public String snombre;
            public String otrosNombres;
            public String papellido;
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

        // POST api/Colaborador/ColaboradorPorPagina
        [HttpPost]
        [Authorize("Colaboradores - Visualizar")]
        public IActionResult ColaboradoresPorPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : null;
                String orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : null;
                String excluir = value.idResponsables != null ? (string)value.idResponsables : null;

                List<Colaborador> colaboradores = ColaboradorDAO.getPagina(pagina, registros, filtro_busqueda, columna_ordenada, orden_direccion, excluir);

                List<stcolaborador> listaColaborador = new List<stcolaborador>();

                foreach (Colaborador colaborador in colaboradores)
                {
                    stcolaborador temp = new stcolaborador();
                    temp.id = colaborador.id;
                    temp.pnombre = colaborador.pnombre;
                    temp.snombre = colaborador.snombre;
                    temp.papellido = colaborador.papellido;
                    temp.sapellido = colaborador.sapellido;
                    temp.cui = colaborador.cui;

                    colaborador.usuarios = UsuarioDAO.getUsuario(colaborador.usuariousuario);
                    temp.usuario = colaborador.usuarios != null ? colaborador.usuarios.usuario : null;

                    colaborador.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(colaborador.ejercicio, colaborador.entidad ?? default(int), colaborador.ueunidadEjecutora);

                    temp.unidadejecutoranombre = colaborador.unidadEjecutoras != null ? colaborador.unidadEjecutoras.nombre : null;
                    temp.ueunidadEjecutora = colaborador.unidadEjecutoras != null ? colaborador.unidadEjecutoras.unidadEjecutora : default(int);
                    temp.entidad = colaborador.unidadEjecutoras != null ? colaborador.unidadEjecutoras.entidadentidad : default(int);

                    if (colaborador.unidadEjecutoras != null)
                    {
                        colaborador.unidadEjecutoras.entidads = EntidadDAO.getEntidad(colaborador.entidad ?? default(int), colaborador.ejercicio);
                        temp.entidadnombre = colaborador.unidadEjecutoras.entidads != null ? colaborador.unidadEjecutoras.entidads.nombre : null;
                    }

                    temp.ejercicio = colaborador.unidadEjecutoras != null ? colaborador.unidadEjecutoras.ejercicio : default(int);
                    temp.usuarioCreo = colaborador.usuarioCreo;
                    temp.usuarioActualizo = colaborador.usuarioActualizo;
                    temp.fechaCreacion = colaborador.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = colaborador.fechaActualizacion != null ? colaborador.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.nombreCompleto = String.Join(" ", temp.pnombre,
                            temp.snombre != null ? temp.snombre : "",
                            temp.papellido != null ? temp.papellido : "",
                            temp.sapellido != null ? temp.sapellido : "");

                    listaColaborador.Add(temp);
                }

                return Ok(new { success = true, colaboradores = listaColaborador });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Colaborador/Colaborador
        [HttpPost]
        [Authorize("Colaboradores - Crear")]
        public IActionResult Colaborador([FromBody]dynamic value)
        {
            try
            {
                ColaboradorValidator validator = new ColaboradorValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    String primerNombre = value.pnombre;
                    String segundoNombre = value.snombre;
                    String primerApellido = value.papellido;
                    String segundoApellido = value.sapellido;
                    long cui = value.cui;
                    int ejercicio = value.ejercicio;
                    int entidad = value.entidad;
                    int codigoUnidadEjecutora = value.ueunidad_ejecutora;
                    String usuarioParametro = value.usuario;

                    Colaborador colaborador = new Colaborador();
                    colaborador.pnombre = primerNombre;
                    colaborador.snombre = segundoNombre;
                    colaborador.papellido = primerApellido;
                    colaborador.sapellido = segundoApellido;
                    colaborador.cui = cui;
                    colaborador.ejercicio = ejercicio;
                    colaborador.entidad = entidad;
                    colaborador.ueunidadEjecutora = codigoUnidadEjecutora;
                    colaborador.usuariousuario = usuarioParametro;
                    colaborador.usuarioCreo = User.Identity.Name;
                    colaborador.fechaCreacion = DateTime.Now;
                    colaborador.estado = 1;

                    bool creado = ColaboradorDAO.guardar(colaborador);

                    return Ok(new { success = creado });
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("2", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/Colaborador/Colaborador/1
        [HttpPut("{id}")]
        [Authorize("Colaboradores - Editar")]
        public IActionResult Colaborador(int id, [FromBody]dynamic value)
        {
            try
            {
                ColaboradorValidator validator = new ColaboradorValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    String primerNombre = value.pnombre;
                    String segundoNombre = value.snombre;
                    String primerApellido = value.papellido;
                    String segundoApellido = value.sapellido;
                    long cui = value.cui;
                    int ejercicio = value.ejercicio;
                    int entidad = value.entidad;
                    int codigoUnidadEjecutora = value.ueunidad_ejecutora;
                    String usuarioParametro = value.usuario;

                    Colaborador colaborador = ColaboradorDAO.getColaborador(id);
                    colaborador.pnombre = primerNombre;
                    colaborador.snombre = segundoNombre;
                    colaborador.papellido = primerApellido;
                    colaborador.sapellido = segundoApellido;
                    colaborador.cui = cui;
                    colaborador.ejercicio = ejercicio;
                    colaborador.entidad = entidad;
                    colaborador.ueunidadEjecutora = codigoUnidadEjecutora;
                    colaborador.usuariousuario = usuarioParametro;
                    colaborador.usuarioActualizo = User.Identity.Name;
                    colaborador.fechaActualizacion = DateTime.Now;

                    bool modificado = ColaboradorDAO.guardar(colaborador);

                    return Ok(new { success = modificado });
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("3", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/Colaborador/Colaborador/1
        [HttpDelete("{id}")]
        [Authorize("Colaboradores - Eliminar")]
        public IActionResult Colaborador(int id)
        {
            try
            {
                Colaborador colaborador = ColaboradorDAO.getColaborador(id);
                colaborador.usuarioActualizo = User.Identity.Name;
                bool borrado = ColaboradorDAO.borrar(colaborador);

                return Ok(new { success = borrado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Colaborador/TotalElementos
        [HttpPost]
        [Authorize("Colaboradores - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                string filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : null;
                String excluir = value.idResponsables != null ? (string)value.idResponsables : null;

                long total = ColaboradorDAO.getTotal(filtro_busqueda, excluir);

                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Colaborador/ValidarUsuario/admin
        [HttpGet("{usuario}")]
        [Authorize("Colaboradores - Visualizar")]
        public IActionResult ValidarUsuario(string usuario)
        {
            try
            {
                bool valido = ColaboradorDAO.validarUsuario(usuario);

                return Ok(new { success = valido });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }

        // GET api/Colaborador/ColaboradorPorId/1
        [HttpGet("{id}")]
        [Authorize("Colaboradores - Visualizar")]
        public IActionResult ColaboradorPorId(int id)
        {
            try
            {
                AsignacionRaci asignacion = AsignacionRaciDAO.getAsignacionPorRolTarea(id, 5, "r", null);
                asignacion.colaboradors = ColaboradorDAO.getColaborador(asignacion.colaboradorid);

                if (asignacion != null && asignacion.colaboradors != null)
                {
                    stcolaborador temp = new stcolaborador();
                    temp.id = asignacion.colaboradors.id;
                    temp.pnombre = asignacion.colaboradors.pnombre;
                    temp.snombre = asignacion.colaboradors.snombre;
                    temp.papellido = asignacion.colaboradors.papellido;
                    temp.sapellido = asignacion.colaboradors.sapellido;
                    temp.cui = asignacion.colaboradors.cui;
                    temp.usuario = asignacion.colaboradors.usuariousuario;

                    asignacion.colaboradors.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(asignacion.colaboradors.ejercicio, asignacion.colaboradors.entidad ?? default(int), asignacion.colaboradors.ueunidadEjecutora);

                    temp.ueunidadEjecutora = asignacion.colaboradors.unidadEjecutoras != null ? asignacion.colaboradors.unidadEjecutoras.unidadEjecutora : default(int);
                    temp.unidadejecutoranombre = asignacion.colaboradors.unidadEjecutoras != null ? asignacion.colaboradors.unidadEjecutoras.nombre : null;
                    temp.ueunidadEjecutora = asignacion.colaboradors.unidadEjecutoras.unidadEjecutora;
                    temp.entidad = asignacion.colaboradors.unidadEjecutoras != null ? asignacion.colaboradors.unidadEjecutoras.entidadentidad : default(int);

                    asignacion.colaboradors.unidadEjecutoras.entidads = EntidadDAO.getEntidad(asignacion.colaboradors.unidadEjecutoras.entidadentidad, asignacion.colaboradors.unidadEjecutoras.ejercicio);

                    temp.entidadnombre = asignacion.colaboradors.unidadEjecutoras.entidads != null ? asignacion.colaboradors.unidadEjecutoras.entidads.nombre : null;
                    temp.ejercicio = asignacion.colaboradors.unidadEjecutoras != null ? asignacion.colaboradors.unidadEjecutoras.ejercicio : default(int);

                    temp.usuarioCreo = asignacion.colaboradors.usuarioCreo;
                    temp.usuarioActualizo = asignacion.colaboradors.usuarioActualizo;
                    temp.fechaCreacion = asignacion.colaboradors.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = asignacion.colaboradors.fechaActualizacion != null ? asignacion.colaboradors.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.nombreCompleto = String.Join(" ", temp.pnombre,
                            temp.snombre != null ? temp.snombre : "",
                            temp.papellido != null ? temp.papellido : "",
                            temp.sapellido != null ? temp.sapellido : "");

                    return Ok(new { success = true, colaborador = temp });
                }
                else
                {
                    return Ok(new { success = false });
                }
            }
            catch (Exception e)
            {
                CLogger.write("7", "ColaboradorController.class", e);
                return BadRequest(500);
            }
        }
    }
}
