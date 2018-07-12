using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;
using FluentValidation.Results;

namespace SProyectoPropiedad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProyectoPropiedadController : Controller
    {
        private class stproyectopropiedad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int datoTipoid;
            public String datoTiponombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        // POST api/ProyectoPropiedad/ProyectoPropiedadPagina
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Visualizar")]
        public IActionResult ProyectoPropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int numeroProyectoPropiedad = value.numeroProyectoPropiedad != null ? (int)value.numeroProyectoPropiedad : default(int);
                string filtro_busqueda = value.filtro_busqueda != null ? value.filtro_busqueda : default(string);
                string columnaOrdenada = value.columnaOrdenada != null ? (string)value.columnaOrdenada : default(string);
                string ordenDireccion = value.ordenDireccion != null ? (string)value.ordenDireccion : default(string);
                
                List<ProyectoPropiedad> proyectopropiedades = ProyectoPropiedadDAO.getProyectoPropiedadesPagina(pagina, numeroProyectoPropiedad,
                         filtro_busqueda, columnaOrdenada, ordenDireccion);
                List<stproyectopropiedad> stproyectopropiedad = new List<stproyectopropiedad>();

                foreach (ProyectoPropiedad proyectopropiedad in proyectopropiedades)
                {
                    stproyectopropiedad temp = new stproyectopropiedad();
                    temp.id = proyectopropiedad.id;
                    temp.nombre = proyectopropiedad.nombre;
                    temp.descripcion = proyectopropiedad.descripcion;
                    proyectopropiedad.datoTipos = DatoTipoDAO.getDatoTipo(proyectopropiedad.datoTipoid);
                    temp.datoTipoid = proyectopropiedad.datoTipos.id;
                    temp.datoTiponombre = proyectopropiedad.datoTipos.nombre;
                    temp.estado = proyectopropiedad.estado;
                    temp.fechaActualizacion = proyectopropiedad.fechaActualizacion != null ? proyectopropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = proyectopropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = proyectopropiedad.usuarioActualizo;
                    temp.usuarioCreo = proyectopropiedad.usuarioCreo;
                    stproyectopropiedad.Add(temp);
                }

                return Ok(new { success = true, proyectopropiedades = stproyectopropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/ProyectoPropiedad/ProyectoPropiedadPaginaPorTipoProy
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Visualizar")]
        public IActionResult ProyectoPropiedadPaginaPorTipoProy([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                int idProyectoPropiedad = value.idProyectoTipo != null ? (int)value.idProyectoTipo : default(int);

                List<ProyectoPropiedad> proyectopropiedades = ProyectoPropiedadDAO.getProyectoPropiedadesPorTipoProyectoPagina(pagina, idProyectoPropiedad);
                List<stproyectopropiedad> stproyectopropiedad = new List<stproyectopropiedad>();
                foreach (ProyectoPropiedad proyectopropiedad in proyectopropiedades)
                {
                    stproyectopropiedad temp = new stproyectopropiedad();
                    temp.id = proyectopropiedad.id;
                    temp.nombre = proyectopropiedad.nombre;
                    temp.descripcion = proyectopropiedad.descripcion;
                    proyectopropiedad.datoTipos = DatoTipoDAO.getDatoTipo(proyectopropiedad.datoTipoid);
                    temp.datoTipoid = proyectopropiedad.datoTipos.id;
                    temp.datoTiponombre = proyectopropiedad.datoTipos.nombre;
                    temp.estado = proyectopropiedad.estado;
                    temp.fechaActualizacion = proyectopropiedad.fechaActualizacion != null ? proyectopropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = proyectopropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = proyectopropiedad.usuarioActualizo;
                    temp.usuarioCreo = proyectopropiedad.usuarioCreo;
                    stproyectopropiedad.Add(temp);
                }

                return Ok(new { success = true, proyectopropiedades  = stproyectopropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/ProyectoPropiedad/ProyectoPropiedadesTotalDisponibles
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Visualizar")]
        public IActionResult ProyectoPropiedadesTotalDisponibles([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : default(int);
                String idsPropiedades = value.idspropiedades != null ? (string)value.idspropiedades : default(string);
                int numeroProyectoPropiedad = value.numeroproyectopropiedad != null ? (int)value.numeroproyectopropiedad : default(int);
                int numeroElementos = value.numeroElementos != null ? (int)value.numeroElementos : default(int);
                List<ProyectoPropiedad> proyectopropiedades = ProyectoPropiedadDAO.getProyectoPropiedadPaginaTotalDisponibles(pagina, numeroProyectoPropiedad, idsPropiedades, numeroElementos);
                List<stproyectopropiedad> stproyectopropiedad = new List<stproyectopropiedad>();
                foreach (ProyectoPropiedad proyectopropiedad in proyectopropiedades)
                {
                    stproyectopropiedad temp = new stproyectopropiedad();
                    temp.id = proyectopropiedad.id;
                    temp.nombre = proyectopropiedad.nombre;
                    temp.descripcion = proyectopropiedad.descripcion;
                    proyectopropiedad.datoTipos = DatoTipoDAO.getDatoTipo(proyectopropiedad.datoTipoid);
                    temp.datoTipoid = proyectopropiedad.datoTipos.id;
                    temp.datoTiponombre = proyectopropiedad.datoTipos.nombre;
                    temp.estado = proyectopropiedad.estado;
                    temp.fechaActualizacion = proyectopropiedad.fechaActualizacion != null ? proyectopropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = proyectopropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = proyectopropiedad.usuarioActualizo;
                    temp.usuarioCreo = proyectopropiedad.usuarioCreo;
                    stproyectopropiedad.Add(temp);
                }

                return Ok(new { success = true, proyectopropiedades = stproyectopropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/ProyectoPropiedad/ProyectoPropiedadPorTipo
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Visualizar")]
        public IActionResult ProyectoPropiedadPorTipo([FromBody]dynamic value)
        {
            try
            {
                int idProyecto = value.idProyecto != null ? (int)value.idProyecto : default(int);
                int idProyectoTipo = value.idProyectoTipo != null ? (int)value.idProyectoTipo : default(int);
                List<ProyectoPropiedad> proyectoPropiedades = ProyectoPropiedadDAO.getProyectoPropiedadesPorTipoProyecto(idProyectoTipo);

                List<Dictionary<string, Object>> campos = new List<Dictionary<string, Object>>();
                foreach (ProyectoPropiedad proyectoPropiedad in proyectoPropiedades)
                {
                    Dictionary<string, Object> campo = new Dictionary<string, Object>();
                    campo.Add("id", proyectoPropiedad.id);
                    campo.Add("nombre", proyectoPropiedad.nombre);
                    campo.Add("tipo", proyectoPropiedad.datoTipoid);
                    ProyectoPropiedadValor proyectoPropiedadValor = ProyectoPropiedadValorDAO.getValorPorProyectoYPropiedad(proyectoPropiedad.id, idProyecto);
                    if (proyectoPropiedadValor != null)
                    {
                        switch ((int)campo["tipo"])
                        {
                            case 1:
                                campo.Add("valor", proyectoPropiedadValor.valorString);
                                break;
                            case 2:
                                campo.Add("valor", proyectoPropiedadValor.valorEntero);
                                break;
                            case 3:
                                campo.Add("valor", proyectoPropiedadValor.valorDecimal);
                                break;
                            case 4:
                                campo.Add("valor", proyectoPropiedadValor.valorEntero == 1 ? true : false);
                                break;
                            case 5:
                                campo.Add("valor", proyectoPropiedadValor.valorTiempo != null ? proyectoPropiedadValor.valorTiempo.Value.ToString("dd/MM/yyyy H:mm:ss") : null);
                                break;
                        }
                    }
                    else
                    {
                        campo.Add("valor", "");
                    }
                    campos.Add(campo);
                }

                List<object> estructuraCamposDinamicos = CFormaDinamica.convertirEstructura(campos);

                return Ok(new { success = true, proyectopropiedades = estructuraCamposDinamicos });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/ProyectoPropiedad/ProyectoPropiedad
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Crear")]
        public IActionResult ProyectoPropiedad([FromBody]dynamic value)
        {
            try
            {
                ProyectoPropiedadValidator validator = new ProyectoPropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int datoTipoId = value.datoTipoid;

                    DatoTipo datoTipo = new DatoTipo();
                    datoTipo.id = datoTipoId;

                    ProyectoPropiedad proyectoPropiedad = new ProyectoPropiedad();
                    proyectoPropiedad.nombre = nombre;
                    proyectoPropiedad.usuarioCreo = User.Identity.Name;
                    proyectoPropiedad.fechaCreacion = DateTime.Now;
                    proyectoPropiedad.estado = 1;
                    proyectoPropiedad.descripcion = descripcion;
                    proyectoPropiedad.datoTipoid = datoTipoId;

                    bool result = ProyectoPropiedadDAO.guardarProyectoPropiedad(proyectoPropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = proyectoPropiedad.id,
                        usuarioCreo = proyectoPropiedad.usuarioCreo,
                        fechaCreacion = proyectoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = proyectoPropiedad.usuarioActualizo,
                        fechaActualizacion = proyectoPropiedad.fechaActualizacion != null ? proyectoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // PUT api/ProyectoPropiedad/ProyectoPropiedad/1
        [HttpPut("{id}")]
        [Authorize("Préstamo o Proyecto Propiedades - Editar")]
        public IActionResult ProyectoPropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                ProyectoPropiedadValidator validator = new ProyectoPropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int datoTipoId = value.datoTipoid;

                    DatoTipo datoTipo = new DatoTipo();
                    datoTipo.id = datoTipoId;

                    ProyectoPropiedad proyectoPropiedad = ProyectoPropiedadDAO.getProyectoPropiedadPorId(id);
                    proyectoPropiedad.nombre = nombre;
                    proyectoPropiedad.descripcion = descripcion;
                    proyectoPropiedad.usuarioActualizo = User.Identity.Name;
                    proyectoPropiedad.fechaActualizacion = DateTime.Now;
                    proyectoPropiedad.datoTipoid = datoTipoId;

                    bool result = ProyectoPropiedadDAO.guardarProyectoPropiedad(proyectoPropiedad);

                    return Ok(new
                    {
                        success = result,
                        id = proyectoPropiedad.id,
                        usuarioCreo = proyectoPropiedad.usuarioCreo,
                        fechaCreacion = proyectoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        usuarioActualizo = proyectoPropiedad.usuarioActualizo,
                        fechaActualizacion = proyectoPropiedad.fechaActualizacion != null ? proyectoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // DELETE api/ProyectoPropiedad/ProyectoPropiedad/1
        [HttpDelete("{id}")]
        [Authorize("Préstamo o Proyecto Propiedades - Eliminar")]
        public IActionResult ProyectoPropiedad(int id)
        {
            try
            {
                ProyectoPropiedad proyectoPropiedad = ProyectoPropiedadDAO.getProyectoPropiedadPorId(id);
                proyectoPropiedad.usuarioActualizo = User.Identity.Name;

                bool eliminado = ProyectoPropiedadDAO.eliminarProyectoPropiedad(proyectoPropiedad);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/ProyectoPropiedad/NumeroProyectoPropiedadesDisponibles
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Visualizar")]
        public IActionResult NumeroProyectoPropiedadesDisponibles([FromBody]dynamic value)
        {
            try
            {
                String idsPropiedades = value.idspropiedades != null ? (string)value.idspropiedades : default(string);

                long total = ProyectoPropiedadDAO.getTotalProyectoPropiedadesDisponibles(idsPropiedades);

                return Ok(new { success = true, totalproyectopropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("8", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        // POST api/ProyectoPropiedad/NumeroProyectoPropiedades
        [HttpPost]
        [Authorize("Préstamo o Proyecto Propiedades - Visualizar")]
        public IActionResult NumeroProyectoPropiedades([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda != null ? (string)value.filtro_busqueda : default(string);

                long total = ProyectoPropiedadDAO.getTotalProyectoPropiedades(filtro_busqueda);
                
                return Ok(new { success = true, totalproyectopropiedades = total });
            }
            catch (Exception e)
            {
                CLogger.write("9", "ProyectoPropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
