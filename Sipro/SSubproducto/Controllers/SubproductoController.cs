using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SSubproducto.Controllers
{
    [Route("api/[controller]")]
    public class SubproductoController : Controller
    {
        private class Stsubproducto
        {
            public int id;
            public int productoid;
            public int subproductoTipoid;
            public String subProductoTipo;
            public int ueunidadEjecutora;
            public int entidadentidad;
            public int ejercicio;
            public String nombreUnidadEjecutora;
            public String entidadnombre;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
            public long? snip;
            public int? programa;
            public int? subprograma;
            public int? proyecto;
            public int? actividad;
            public int? obra;
            public int? renglon;
            public int? ubicacionGeografica;
            public int duracion;
            public String duracionDimension;
            public String fechaInicio;
            public String fechaFin;
            public String latitud;
            public String longitud;
            public decimal costo;
            public int acumulacionCostoid;
            public String acumulacionCostoNombre;
            public bool tieneHijos;
            public String fechaInicioReal;
            public String fechaFinReal;
            public String fechaElegibilidad;
            public String fechaCierre;
            public int inversionNueva;
            public int orden;
            public String treepath;
            public int nivel;
        }

        private class Stdatadinamico
        {
            public String id;
            public String tipo;
            public String label;
            public String valor;
            public String valor_f;
        }

        [HttpPost]
        public IActionResult SubproductoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                int productoid = value.productoid != null ? (int)value.productoid : default(int);
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<Subproducto> subproductos = SubproductoDAO.getSubproductosPagina(pagina, registros, productoid, filtro_busqueda, columna_ordenada, 
                    orden_direccion, User.Identity.Name);

                List<Stsubproducto> lstsubproductos = new List<Stsubproducto>();
                foreach (Subproducto subproducto in subproductos)
                {
                    Stsubproducto temp = new Stsubproducto();
                    temp.id = subproducto.id;
                    temp.nombre = subproducto.nombre;
                    temp.descripcion = subproducto.descripcion;
                    temp.programa = subproducto.programa;
                    temp.subprograma = subproducto.subprograma;
                    temp.proyecto = subproducto.proyecto;
                    temp.obra = subproducto.obra;
                    temp.actividad = subproducto.actividad;
                    temp.renglon = subproducto.renglon;
                    temp.ubicacionGeografica = subproducto.ubicacionGeografica;
                    temp.duracion = subproducto.duracion;
                    temp.duracionDimension = subproducto.duracionDimension;
                    temp.fechaInicio = subproducto.fechaInicio.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaFin = subproducto.fechaFin != null ? subproducto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.snip = subproducto.snip;
                    temp.estado = subproducto.estado;
                    temp.usuarioCreo = subproducto.usuarioCreo;
                    temp.usuarioActualizo = subproducto.usuarioActualizo;
                    temp.fechaCreacion = subproducto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = subproducto.fechaActualizacion != null ? subproducto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.latitud = subproducto.latitud;
                    temp.longitud = subproducto.longitud;
                    temp.costo = subproducto.costo ?? default(decimal);

                    subproducto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(subproducto.acumulacionCostoid));

                    temp.acumulacionCostoid = Convert.ToInt32(subproducto.acumulacionCostoid);
                    temp.acumulacionCostoNombre = subproducto.acumulacionCostos.nombre;

                    subproducto.subproductoTipos = SubproductoTipoDAO.getSubproductoTipo(subproducto.subproductoTipoid);

                    if (subproducto.subproductoTipos != null)
                    {
                        temp.subproductoTipoid = subproducto.subproductoTipos.id;
                        temp.subProductoTipo = subproducto.subproductoTipos.nombre;
                    }

                    subproducto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.ejercicio, subproducto.entidad ?? default(int), subproducto.ueunidadEjecutora);
                    subproducto.productos = ProductoDAO.getProductoPorId(subproducto.productoid);
                    subproducto.productos.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(subproducto.productos.ejercicio, subproducto.productos.entidad ?? default(int), subproducto.productos.ueunidadEjecutora);

                    if (subproducto.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subproducto.ueunidadEjecutora;
                        temp.nombreUnidadEjecutora = subproducto.unidadEjecutoras.nombre;
                        temp.entidadentidad = subproducto.entidad ?? default(int);
                        temp.ejercicio = subproducto.ejercicio;

                        subproducto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.entidad ?? default(int), subproducto.ejercicio);
                        temp.entidadnombre = subproducto.unidadEjecutoras.entidads.nombre;
                    }
                    else if (subproducto.productos.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = subproducto.productos.ueunidadEjecutora;
                        temp.nombreUnidadEjecutora = subproducto.productos.unidadEjecutoras.nombre;
                        temp.entidadentidad = subproducto.productos.entidad ?? default(int);
                        temp.ejercicio = subproducto.productos.ejercicio;
                        subproducto.productos.unidadEjecutoras.entidads = EntidadDAO.getEntidad(subproducto.productos.entidad ?? default(int), subproducto.productos.ejercicio);
                        temp.entidadnombre = subproducto.productos.unidadEjecutoras.entidads.nombre;
                    }

                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 4);

                    temp.fechaInicioReal = subproducto.fechaInicioReal != null ? subproducto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = subproducto.fechaFinReal != null ? subproducto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.inversionNueva = subproducto.inversionNueva;
                }
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("1", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult Subproducto([FromBody]dynamic value)
        {
            try
            {
                SubproductoValidator validator = new SubproductoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    Subproducto subproducto = new Subproducto();

                    return Ok();
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Subproducto(int id, [FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("3", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Subproducto(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("4", "SubproductoController.class", e);
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
                CLogger.write("5", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerSubproductoPorId(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("6", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SubproductoPorId(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("7", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {               
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("8", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        public IActionResult Historia(int id, int version)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("9", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        public IActionResult ValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                CLogger.write("10", "SubproductoController.class", e);
                return BadRequest(500);
            }
        }
    }
}

