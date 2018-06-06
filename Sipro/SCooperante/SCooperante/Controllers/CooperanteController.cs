using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiproModelCore.Models;
using SiproDAO.Dao;
using Utilities;
using Microsoft.AspNetCore.Cors;
using FluentValidation.Results;

namespace SCooperante.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
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

        // GET api/Cooperante/Cooperantes
        [HttpGet]
        [Authorize("Cooperantes - Visualizar")]
        public IActionResult Cooperantes()
        {
            try
            {
                List<Cooperante> cooperantes = CooperanteDAO.getCooperantes();

                if (cooperantes != null)
                {
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

                    return Ok(new { success = true, cooperantes = stcooperantes });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("1", "CooperanteController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Cooperante/Cooperantes
        [HttpPost]
        [Authorize("Cooperantes - Crear")]
        public IActionResult Cooperante([FromBody]dynamic value)
        {
            try
            {
                CooperanteValidator validator = new CooperanteValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Cooperante cooperante = new Cooperante();
                    cooperante.codigo = value.codigo;
                    cooperante.descripcion = value.descripcion;
                    cooperante.ejercicio = value.ejercicio;
                    cooperante.estado = value.estado;
                    cooperante.fechaCreacion = DateTime.Now;
                    cooperante.nombre = value.nombre;
                    cooperante.siglas = value.siglas;
                    cooperante.usuarioCreo = User.Identity.Name;

                    bool result = CooperanteDAO.guardarCooperante(cooperante);

                    if (result)
                    {
                        return Ok(new
                        {
                            success = true,
                            id = cooperante.codigo,
                            usuarioCreo = cooperante.usuarioCreo,
                            fechaCreacion = cooperante.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            usuarioactualizo = cooperante.usuarioActualizo != null ? cooperante.usuarioActualizo : "",
                            fechaactualizacion = cooperante.fechaActualizacion != null ? cooperante.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : ""
                        });
                    }
                    else
                        return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "CooperanteController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Cooperante/Cooperantes/codigo
        [HttpPut("{codigo}")]
        [Authorize("Cooperantes - Editar")]
        public IActionResult CooperanteA(int codigo, [FromBody]dynamic value)
        {
            try
            {
                CooperanteValidator validator = new CooperanteValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo(codigo);
                    cooperante.descripcion = value["descripcion"] != null ? (string)value.descripcion : default(string);
                    cooperante.ejercicio = value.ejercicio != null ? (int)value.ejercicio : default(int);
                    cooperante.estado = value.estado != null ? (int)value.estado : default(int);
                    cooperante.fechaActualizacion = DateTime.Now;
                    cooperante.nombre = value.nombre != null ? (string)value.nombre : default(string);
                    cooperante.siglas = value.siglas != null ? (string)value.siglas : default(string);
                    cooperante.usuarioActualizo = User.Identity.Name;

                    bool result = CooperanteDAO.guardarCooperante(cooperante);

                    if (result)
                    {
                        return Ok(new
                        {
                            success = true,
                            id = cooperante.codigo,
                            usuarioCreo = cooperante.usuarioCreo,
                            fechaCreacion = cooperante.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                            usuarioactualizo = cooperante.usuarioActualizo != null ? cooperante.usuarioActualizo : "",
                            fechaactualizacion = cooperante.fechaActualizacion != null ? cooperante.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : ""
                        });
                    }
                    else
                        return Ok(new { success = false });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "CooperanteController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/Cooperante/Cooperantes/codigo
        [HttpDelete("{codigo}")]
        [Authorize("Cooperantes - Eliminar")]        
        public IActionResult Cooperantes(int codigo)
        {
            try
            {
                if (codigo > 0)
                {
                    Cooperante cooperante = CooperanteDAO.getCooperantePorCodigo(codigo);
                    cooperante.usuarioActualizo = User.Identity.Name;

                    bool eliminado = CooperanteDAO.eliminarCooperante(cooperante);

                    return Ok(new { success = (eliminado ? true : false) });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("4", "CooperanteController.class", e);
                return BadRequest(500);
            }            
        }

        // POST api/Cooperante/CooperantesPagina
        [HttpPost]
        [Authorize("Cooperantes - Visualizar")]
        public IActionResult CooperantesPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numerocooperantes = value.numerocooperantes != null ? (int)value.numerocooperantes : default(int);
                string filtro_codigo = value.filtro_codigo != null ? value.filtro_codigo : default(string);
                string filtro_nombre = value.filtro_nombre != null ? (string)value.filtro_nombre : default(string);
                string filtro_usuario_creo = value.filtro_usuario_creo != null ? (string)value.filtro_usuario_creo : default(string);
                string filtro_fecha_creacion = value.filtro_fecha_creacion != null ? (string)value.filtro_fecha_creacion : default(string);
                string columna_ordenada = value.columna_ordenada != null ? (string)value.columna_ordenada : default(string);
                string orden_direccion = value.orden_direccion != null ? (string)value.orden_direccion : default(string);

                List<Cooperante> cooperantes = CooperanteDAO.getCooperantesPagina(pagina, numerocooperantes, filtro_codigo,
                filtro_nombre, filtro_usuario_creo, filtro_fecha_creacion, columna_ordenada, orden_direccion);

                if (cooperantes != null)
                {
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

                    return Ok(new { success = true, cooperantes = stcooperantes });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("5", "CooperanteController.class", e);
                return BadRequest(500);
            }            
        }

        // POST api/Cooperante/TotalCooperantes
        [HttpPost]
        [Authorize("Cooperantes - Visualizar")]
        public IActionResult TotalCooperantes([FromBody]dynamic value)
        {
            try
            {
                string filtro_codigo = value.filtro_codigo != null ? value.filtro_codigo : default(string);
                string filtro_nombre = value.filtro_nombre != null ? (string)value.filtro_nombre : default(string);
                string filtro_usuario_creo = value.filtro_usuario_creo != null ? (string)value.filtro_usuario_creo : default(string);
                string filtro_fecha_creacion = value.filtro_fecha_creacion != null ? (string)value.filtro_fecha_creacion : default(string);
                
                long total = CooperanteDAO.getTotalCooperantes(filtro_codigo, filtro_nombre, filtro_usuario_creo, 
                    filtro_fecha_creacion);

                return Ok(new { success = true, totalcooperantes = total });
            }
            catch (Exception e)
            {
                CLogger.write("6", "CooperanteController.class", e);
                return BadRequest(500);
            }            
        }
    }
}