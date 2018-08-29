using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubproductoPropiedad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class SubproductoPropiedadController : Controller
    {
        private class StSubproductoPropiedad
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int datoTipoid;
            public String datoTipoNombre;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
        }

        [HttpPost]
        [Authorize("Subproducto Propiedades - Visualizar")]
        public IActionResult SubproductoPropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<SubproductoPropiedad> subProductoPropiedades = SubproductoPropiedadDAO.getPagina(pagina, registros, filtro_busqueda, columna_ordenada, 
                    orden_direccion);

                List<StSubproductoPropiedad> lstSubproductoPropiedades = new List<StSubproductoPropiedad>();
                foreach (SubproductoPropiedad subproductoPropiedad in subProductoPropiedades)
                {
                    StSubproductoPropiedad temp = new StSubproductoPropiedad();
                    temp.id = subproductoPropiedad.id;
                    temp.nombre = subproductoPropiedad.nombre;

                    subproductoPropiedad.datoTipos = DatoTipoDAO.getDatoTipo(subproductoPropiedad.datoTipoid);
                    temp.datoTipoid = subproductoPropiedad.datoTipoid;
                    temp.datoTipoNombre = subproductoPropiedad.datoTipos.nombre;
                    temp.usuarioCreo = subproductoPropiedad.usuarioCreo;
                    temp.usuarioActualizo = subproductoPropiedad.usuarioActualizo;
                    temp.fechaCreacion = subproductoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = subproductoPropiedad.fechaActualizacion != null ? subproductoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.estado = subproductoPropiedad.estado;
                    lstSubproductoPropiedades.Add(temp);
                }

                return Ok(new { success = true, subproductoPropiedades  = lstSubproductoPropiedades });
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproducto Propiedades - Crear")]
        public IActionResult SubproductoPropiedad([FromBody]dynamic value)
        {
            try
            {
                SubproductoPropiedadValidator validator = new SubproductoPropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubproductoPropiedad subproductoPropiedad = new SubproductoPropiedad();
                    subproductoPropiedad.nombre = value.nombre;
                    subproductoPropiedad.descripcion = value.descripcion;
                    subproductoPropiedad.usuarioCreo = User.Identity.Name;
                    subproductoPropiedad.fechaCreacion = DateTime.Now;
                    subproductoPropiedad.estado = 1;
                    subproductoPropiedad.datoTipoid = value.datoTipoid;

                    bool guardado = SubproductoPropiedadDAO.guardarSubproductoPropiedad(subproductoPropiedad);

                    return Ok(new
                    {
                        success = guardado,
                        id = subproductoPropiedad.id,
                        usuarioCreo = subproductoPropiedad.usuarioCreo,
                        usuarioActualizo = subproductoPropiedad.usuarioActualizo,
                        fechaCreacion = subproductoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subproductoPropiedad.fechaActualizacion != null ? subproductoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null,
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Subproducto Propiedades - Editar")]
        public IActionResult SubproductoPropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                SubproductoPropiedadValidator validator = new SubproductoPropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    SubproductoPropiedad subproductoPropiedad = SubproductoPropiedadDAO.getSubproductoPropiedadPorId(id);
                    subproductoPropiedad.nombre = value.nombre;
                    subproductoPropiedad.descripcion = value.descripcion;
                    subproductoPropiedad.usuarioActualizo = User.Identity.Name;
                    subproductoPropiedad.fechaActualizacion = DateTime.Now;
                    subproductoPropiedad.datoTipoid = value.datoTipoid;

                    bool guardado = SubproductoPropiedadDAO.guardarSubproductoPropiedad(subproductoPropiedad);

                    return Ok(new
                    {
                        success = guardado,
                        id = subproductoPropiedad.id,
                        usuarioCreo = subproductoPropiedad.usuarioCreo,
                        usuarioActualizo = subproductoPropiedad.usuarioActualizo,
                        fechaCreacion = subproductoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = subproductoPropiedad.fechaActualizacion != null ? subproductoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null,
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Subproducto Propiedades - Eliminar")]
        public IActionResult SubproductoPropiedad(int id)
        {
            try
            {
                SubproductoPropiedad subproductoPropiedad = SubproductoPropiedadDAO.getSubproductoPropiedadPorId(id);
                subproductoPropiedad.usuarioActualizo = User.Identity.Name;

                bool eliminado = SubproductoPropiedadDAO.eliminarSubproductoPropiedad(subproductoPropiedad);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Subproducto Propiedades - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;
                long total = SubproductoPropiedadDAO.getTotal(filtro_busqueda);

                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{subproductoId}/{subproductoTipoId}")]
        [Authorize("Subproducto Propiedades - Visualizar")]
        public IActionResult SubproductoPropiedadPorTipo(int subproductoId, int subproductoTipoId)
        {
            try
            {
                List<SubproductoPropiedad> subproductopropiedades = SubproductoPropiedadDAO.getSubproductoPropiedadesPorTipo(subproductoTipoId);

                List<Dictionary<String, Object>> campos = new List<Dictionary<String, Object>>();
                foreach (SubproductoPropiedad subProductopropiedad in subproductopropiedades)
                {
                    Dictionary<String, Object> campo = new Dictionary<String, Object>();
                    campo.Add("id", subProductopropiedad.id);
                    campo.Add("nombre", subProductopropiedad.nombre);
                    campo.Add("tipo", subProductopropiedad.datoTipoid);
                    SubproductoPropiedadValor subproductoPropiedadValor = SubproductoPropiedadValorDAO.getValorPorSubProdcutoYPropiedad(subProductopropiedad.id, subproductoId);
                    if (subproductoPropiedadValor != null)
                    {
                        switch (subProductopropiedad.datoTipoid)
                        {
                            case 1:
                                campo.Add("valor", subproductoPropiedadValor.valorString);
                                break;
                            case 2:
                                campo.Add("valor", subproductoPropiedadValor.valorEntero);
                                break;
                            case 3:
                                campo.Add("valor", subproductoPropiedadValor.valorDecimal);
                                break;
                            case 4:
                                campo.Add("valor", subproductoPropiedadValor.valorEntero == 1 ? true : false);
                                break;
                            case 5:
                                campo.Add("valor", subproductoPropiedadValor.valorTiempo != null ? subproductoPropiedadValor.valorTiempo.Value.ToString("dd/MM/yyyy H:mm:ss") : null);
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

                return Ok(new { success = true, subproductopropiedades = estructuraCamposDinamicos });
            }
            catch (Exception e)
            {
                CLogger.write("5", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idSubproductoTipo}")]
        [Authorize("Subproducto Propiedades - Visualizar")]
        public IActionResult SubProductoPropiedadPorTipoSubProducto(int idSubproductoTipo)
        {
            try
            {
                List<SubproductoPropiedad> subproductopropiedades = SubproductoPropiedadDAO.getSubproductoPropiedadesPorTipo(idSubproductoTipo);
                List<StSubproductoPropiedad> stsubproductopropiedad = new List<StSubproductoPropiedad>();
                foreach (SubproductoPropiedad subproductopropiedad in subproductopropiedades)
                {
                    StSubproductoPropiedad temp = new StSubproductoPropiedad();
                    temp.id = subproductopropiedad.id;
                    temp.nombre = subproductopropiedad.nombre;
                    temp.descripcion = subproductopropiedad.descripcion;

                    subproductopropiedad.datoTipos = DatoTipoDAO.getDatoTipo(subproductopropiedad.datoTipoid);

                    temp.datoTipoid = subproductopropiedad.datoTipoid;
                    temp.datoTipoNombre = subproductopropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = subproductopropiedad.fechaActualizacion != null ? subproductopropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = subproductopropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = subproductopropiedad.usuarioActualizo;
                    temp.usuarioCreo = subproductopropiedad.usuarioCreo;
                    stsubproductopropiedad.Add(temp);
                }

                return Ok(new { success = true, subproductopropiedades = stsubproductopropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubproductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
