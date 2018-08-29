using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SProducto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProductoController : Controller
    {
        private class Stproducto
        {
            public int id;
            public String nombre;
            public String descripcion;
            public int componenteid;
            public String componenteNombre;
            public int subcomponenteid;
            public String subcomponenteNombre;
            public int productoTipoid;
            public String productoTipoNombre;
            public int ueunidadEjecutora;
            public int entidad;
            public int ejercicio;
            public String nombreUnidadEjecutora;
            public String entidadnombre;
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
            public int estado;
            public String fechaCreacion;
            public String usuarioCreo;
            public String fechaActualizacion;
            public String usuarioActualizo;
            public String latitud;
            public String longitud;
            public int peso;
            public decimal costo;
            public int acumulacionCostoid;
            public String acumulacionCostoNombre;
            public bool tieneHijos;
            public String fechaInicioReal;
            public String fechaFinReal;
            public String fechaElegibilidad;
            public String fechaCierre;
            public int inversionNueva;
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
        [Authorize("Productos - Visualizar")]
        public IActionResult ProductoPagina([FromBody]dynamic value)
        {
            try
            {
                int componenteid = value.componenteid != null ? (int)value.componenteid : 0;
                int subcomponenteid = value.subcomponenteid != null ? (int)value.subcomponenteid : 0;
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? value.registros : 20;
                String filtro_busqueda = (string)value.filtro_busqueda;
                String columna_ordenada = (string)value.columna_ordenada;
                String orden_direccion = (string)value.orden_direccion;

                List<Producto> productos = ProductoDAO.getProductosPagina(pagina, registros, componenteid, subcomponenteid, filtro_busqueda, columna_ordenada, 
                    orden_direccion, User.Identity.Name);
                List<Stproducto> listaProducto = new List<Stproducto>();

                String fechaElegibilidad = null;
                String fechaCierre = null;

                if (productos != null && productos.Count > 0)
                {
                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(productos[0].treepath);
                    if (proyecto != null)
                    {
                        fechaElegibilidad = proyecto.fechaElegibilidad != null ? proyecto.fechaElegibilidad.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                        fechaCierre = proyecto.fechaCierre != null ? proyecto.fechaCierre.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    }
                }

                foreach (Producto producto in productos)
                {
                    Stproducto temp = new Stproducto();
                    temp.id = producto.id;
                    temp.nombre = producto.nombre;
                    temp.descripcion = producto.descripcion;
                    temp.programa = producto.programa;
                    temp.subprograma = producto.subprograma;
                    temp.proyecto = producto.proyecto;
                    temp.obra = producto.obra;
                    temp.actividad = producto.actividad;
                    temp.renglon = producto.renglon;
                    temp.ubicacionGeografica = producto.ubicacionGeografica;
                    temp.duracion = producto.duracion;
                    temp.duracionDimension = producto.duracionDimension;
                    temp.fechaInicio = producto.fechaInicio != null ? producto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFin = producto.fechaFin != null ? producto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.snip = producto.snip;
                    temp.estado = producto.estado ?? default(int);
                    temp.usuarioCreo = producto.usuarioCreo;
                    temp.usuarioActualizo = producto.usuarioActualizo;
                    temp.fechaCreacion = producto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = producto.fechaActualizacion != null ? producto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.latitud = producto.latitud;
                    temp.longitud = producto.longitud;
                    temp.peso = producto.peso ?? default(int);
                    temp.costo = producto.costo ?? default(decimal);

                    producto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(producto.acumulacionCostoid));

                    temp.acumulacionCostoid = Convert.ToInt32(producto.acumulacionCostoid);
                    temp.acumulacionCostoNombre = producto.acumulacionCostos != null ? producto.acumulacionCostos.nombre : null;

                    producto.productoTipos = ProductoTipoDAO.getProductoTipo(producto.productoTipoid);
                    temp.productoTipoid = producto.productoTipoid;
                    temp.productoTipoNombre = producto.productoTipos.nombre;
                    
                    if (producto.componenteid != null)
                    {
                        producto.componentes = ComponenteDAO.getComponentePorId(producto.componenteid ?? default(int), null);
                        temp.componenteid = producto.componenteid ?? default(int);
                        temp.componenteNombre = producto.componentes.nombre;

                        producto.componentes.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.componentes.ejercicio, producto.componentes.entidad ?? default(int), producto.componentes.ueunidadEjecutora);
                        if (producto.componentes.unidadEjecutoras != null)
                        {
                            temp.ueunidadEjecutora = producto.componentes.ueunidadEjecutora;
                            temp.entidad = producto.componentes.entidad ?? default(int);
                            temp.ejercicio = producto.componentes.ejercicio;
                            temp.nombreUnidadEjecutora = producto.componentes.unidadEjecutoras.nombre;

                            producto.componentes.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.componentes.entidad ?? default(int), producto.componentes.ejercicio);
                            temp.entidadnombre = producto.componentes.unidadEjecutoras.entidads.nombre;
                        }
                    }

                    if (producto.subcomponenteid != null)
                    {
                        producto.subcomponentes = SubComponenteDAO.getSubComponentePorId(producto.subcomponenteid ?? default(int), null);
                        temp.subcomponenteid = producto.subcomponenteid ?? default(int);
                        temp.subcomponenteNombre = producto.subcomponentes != null ? producto.subcomponentes.nombre : null;

                        producto.subcomponentes.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.subcomponentes.ejercicio ?? default(int), producto.subcomponentes.entidad ?? default(int), producto.subcomponentes.ueunidadEjecutora ?? default(int));
                        if (producto.subcomponentes.unidadEjecutoras != null)
                        {
                            temp.ueunidadEjecutora = producto.subcomponentes.ueunidadEjecutora ?? default(int);
                            temp.entidad = producto.subcomponentes.entidad ?? default(int);
                            temp.ejercicio = producto.subcomponentes.ejercicio ?? default(int);
                            temp.nombreUnidadEjecutora = producto.subcomponentes.unidadEjecutoras.nombre;

                            producto.subcomponentes.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.subcomponentes.entidad ?? default(int), producto.subcomponentes.ejercicio ?? default(int));
                            temp.entidadnombre = producto.subcomponentes.unidadEjecutoras.entidads.nombre;
                        }
                    }

                    producto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.ejercicio, producto.entidad ?? default(int), producto.ueunidadEjecutora);
                    if (producto.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = producto.ueunidadEjecutora;
                        temp.entidad = producto.entidad ?? default(int);
                        temp.ejercicio = producto.ejercicio;
                        temp.nombreUnidadEjecutora = producto.unidadEjecutoras.nombre;

                        producto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.entidad ?? default(int), producto.ejercicio);
                        temp.entidadnombre = producto.unidadEjecutoras.entidads.nombre;
                    }
                    
                    temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 3);
                    temp.fechaInicioReal = producto.fechaInicioReal != null ? producto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaFinReal = producto.fechaFinReal != null ? producto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                    temp.fechaElegibilidad = fechaElegibilidad;
                    temp.fechaCierre = fechaCierre;
                    temp.inversionNueva = producto.inversionNueva ?? default(int);

                    listaProducto.Add(temp);
                }

                return Ok(new { success = true, productos = listaProducto });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Productos - Crear")]
        public IActionResult Producto([FromBody]dynamic value)
        {            
            try
            {                
                ProductoValidator validator = new ProductoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool ret = false;
                    Producto producto = new Producto();
                    producto.nombre = value.nombre;
                    producto.descripcion = value.descripcion;
                    producto.componenteid = value.componenteid;
                    producto.subcomponenteid = value.subcomponenteid;
                    producto.productoTipoid = value.productoTipoid;
                    producto.ueunidadEjecutora = value.ueunidadEjecutora;
                    producto.entidad = value.entidad;
                    producto.ejercicio = value.ejercicio;
                    producto.snip = value.snip;
                    producto.programa = value.programa;
                    producto.subprograma = value.subprograma;
                    producto.proyecto = value.proyecto;
                    producto.obra = value.obra;
                    producto.renglon = value.renglon;
                    producto.ubicacionGeografica = value.ubicacionGeografica;
                    producto.actividad = value.actividad;
                    producto.latitud = value.latitud;
                    producto.longitud = value.longitud;
                    producto.peso = value.peso;
                    producto.costo = value.costo;
                    producto.acumulacionCostoid = value.acumulacionCostoid;
                    producto.fechaInicio = value.fechaInicio;

                    DateTime fechaFin;
                    DateTime.TryParse((string)value.fechaFin, out fechaFin);
                    producto.fechaFin = fechaFin;

                    producto.duracion = value.duracion;
                    producto.duracionDimension = value.duracionDimension;
                    producto.inversionNueva = value.inversionNueva;
                    producto.acumulacionCostoid = value.acumulacionCostoid;
                    producto.estado = 1;
                    producto.usuarioCreo = User.Identity.Name;
                    producto.fechaCreacion = DateTime.Now;

                    DateTime fechaInicioReal;
                    DateTime.TryParse((string)value.fechaInicioReal, out fechaInicioReal);
                    producto.fechaInicioReal = fechaInicioReal;

                    DateTime fechaFinReal;
                    DateTime.TryParse((string)value.fechaFinReal, out fechaFinReal);
                    producto.fechaFinReal = fechaFinReal;

                    ret = ProductoDAO.guardarProducto(producto, true);

                    if (ret)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!producto.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(producto.id, 3);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (producto.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            JArray pagosArreglo = JArray.Parse((string)value.pagosPlanificados);
                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];
                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);
                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"].ToString()) : default(decimal);

                                PagoPlanificado pagoPlanificado = new PagoPlanificado();
                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = producto.id;
                                pagoPlanificado.objetoTipo = 3;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                ret = ret && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (ret)
                    {
                        List<ProductoPropiedad> productoPropiedades = ProductoPropiedadDAO.getProductoPropiedadesPorTipo(producto.productoTipoid);

                        foreach (ProductoPropiedad productoPropiedad in productoPropiedades)
                        {
                            ProductoPropiedadValor productoPropVal = ProductoPropiedadValorDAO.getValorPorProdcutoYPropiedad(productoPropiedad.id, producto.id);
                            if (productoPropVal != null)
                                ret = ret && ProductoPropiedadValorDAO.eliminarProductoPropiedadValor(productoPropVal.productoPropiedadid, productoPropVal.productoid);
                        }

                        JArray datosDinamicos = JArray.Parse((string)value.camposDinamicos);

                        for (int i = 0; i < datosDinamicos.Count; i++)
                        {
                            JObject data = (JObject)datosDinamicos[i];

                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                ProductoPropiedad productoPropiedad = ProductoPropiedadDAO.getProductoPropiedad(Convert.ToInt32(data["id"]));
                                ProductoPropiedadValor valor = new ProductoPropiedadValor();
                                valor.productos = producto;
                                valor.productoid = producto.id;
                                valor.productoPropiedads = productoPropiedad;
                                valor.productoPropiedadid = productoPropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;

                                switch (productoPropiedad.datoTipoid)
                                {
                                    case 1:
                                        valor.valorString = data["valor"].ToString();
                                        break;
                                    case 2:
                                        valor.valorEntero = Convert.ToInt32(data["valor"].ToString());
                                        break;
                                    case 3:
                                        valor.valorDecimal = Convert.ToDecimal(data["valor"].ToString());
                                        break;
                                    case 4:
                                        valor.valorEntero = data["valor"].ToString() == "true" ? 1 : 0;
                                        break;
                                    case 5:
                                        valor.valorTiempo = Convert.ToDateTime(data["valor_f"].ToString());
                                        break;
                                }
                                ret = (ret && ProductoPropiedadValorDAO.guardarProductoPropiedadValor(valor));
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = ret,
                        id = producto.id,
                        usuarioCreo = producto.usuarioCreo,
                        usuarioActualizo = producto.usuarioActualizo,
                        fechaCreacion = producto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = producto.fechaActualizacion != null ? producto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Productos - Editar")]
        public IActionResult Producto(int id, [FromBody]dynamic value)
        {
            try
            {                
                ProductoValidator validator = new ProductoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    bool ret = false;
                    Producto producto = ProductoDAO.getProductoPorId(id);
                    producto.nombre = value.nombre;
                    producto.descripcion = value.descripcion;
                    producto.componenteid = value.componenteid;
                    producto.subcomponenteid = value.subcomponenteid;
                    producto.productoTipoid = value.productoTipoid;
                    producto.ueunidadEjecutora = value.ueunidadEjecutora;
                    producto.entidad = value.entidad;
                    producto.ejercicio = value.ejercicio;
                    producto.snip = value.snip;
                    producto.programa = value.programa;
                    producto.subprograma = value.subprograma;
                    producto.proyecto = value.proyecto;
                    producto.obra = value.obra;
                    producto.renglon = value.renglon;
                    producto.ubicacionGeografica = value.ubicacionGeografica;
                    producto.actividad = value.actividad;
                    producto.latitud = value.latitud;
                    producto.longitud = value.longitud;
                    producto.peso = value.peso;
                    producto.costo = value.costo;
                    producto.acumulacionCostoid = value.acumulacionCostoid;
                    producto.fechaInicio = value.fechaInicio;

                    DateTime fechaFin;
                    DateTime.TryParse((string)value.fechaFin, out fechaFin);
                    producto.fechaFin = fechaFin;

                    DateTime fechaInicioReal;
                    DateTime.TryParse((string)value.fechaInicioReal, out fechaInicioReal);
                    producto.fechaInicioReal = fechaInicioReal;

                    DateTime fechaFinReal;
                    DateTime.TryParse((string)value.fechaFinReal, out fechaFinReal);
                    producto.fechaFinReal = fechaFinReal;

                    producto.duracion = value.duracion;
                    producto.duracionDimension = value.duracionDimension;
                    producto.inversionNueva = value.inversionNueva;
                    producto.acumulacionCostoid = value.acumulacionCostoid;
                    producto.usuarioActualizo = User.Identity.Name;
                    producto.fechaActualizacion = DateTime.Now;

                    ret = ProductoDAO.guardarProducto(producto, true);

                    if (ret)
                    {
                        String pagosPlanificados = value.pagosPlanificados;
                        if (!producto.acumulacionCostoid.Equals(2) || pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            List<PagoPlanificado> pagosActuales = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(producto.id, 3);
                            foreach (PagoPlanificado pagoTemp in pagosActuales)
                            {
                                PagoPlanificadoDAO.eliminarTotalPagoPlanificado(pagoTemp);
                            }
                        }

                        if (producto.acumulacionCostoid.Equals(2) && pagosPlanificados != null && pagosPlanificados.Length > 0)
                        {
                            JArray pagosArreglo = JArray.Parse((string)value.pagosPlanificados);
                            for (int i = 0; i < pagosArreglo.Count; i++)
                            {
                                JObject objeto = (JObject)pagosArreglo[i];
                                DateTime fechaPago = objeto["fechaPago"] != null ? Convert.ToDateTime(objeto["fechaPago"].ToString()) : default(DateTime);
                                decimal monto = objeto["pago"] != null ? Convert.ToDecimal(objeto["pago"].ToString()) : default(decimal);

                                PagoPlanificado pagoPlanificado = new PagoPlanificado();
                                pagoPlanificado.fechaPago = fechaPago;
                                pagoPlanificado.pago = monto;
                                pagoPlanificado.objetoId = producto.id;
                                pagoPlanificado.objetoTipo = 3;
                                pagoPlanificado.usuarioCreo = User.Identity.Name;
                                pagoPlanificado.fechaCreacion = DateTime.Now;
                                pagoPlanificado.estado = 1;
                                ret = ret && PagoPlanificadoDAO.Guardar(pagoPlanificado);
                            }
                        }
                    }

                    if (ret)
                    {
                        List<ProductoPropiedad> productoPropiedades = ProductoPropiedadDAO.getProductoPropiedadesPorTipo(producto.productoTipoid);

                        foreach (ProductoPropiedad productoPropiedad in productoPropiedades)
                        {
                            ProductoPropiedadValor productoPropVal = ProductoPropiedadValorDAO.getValorPorProdcutoYPropiedad(productoPropiedad.id, producto.id);
                            if (productoPropVal != null)
                                ret = ret && ProductoPropiedadValorDAO.eliminarProductoPropiedadValor(productoPropVal.productoPropiedadid, productoPropVal.productoid);
                        }

                        JArray datosDinamicos = JArray.Parse((string)value.camposDinamicos);

                        for (int i = 0; i < datosDinamicos.Count; i++)
                        {
                            JObject data = (JObject)datosDinamicos[i];

                            if (data["valor"] != null && data["valor"].ToString().Length > 0 && data["valor"].ToString().CompareTo("null") != 0)
                            {
                                ProductoPropiedad productoPropiedad = ProductoPropiedadDAO.getProductoPropiedad(Convert.ToInt32(data["id"]));
                                ProductoPropiedadValor valor = new ProductoPropiedadValor();
                                valor.productos = producto;
                                valor.productoid = producto.id;
                                valor.productoPropiedads = productoPropiedad;
                                valor.productoPropiedadid = productoPropiedad.id;
                                valor.usuarioCreo = User.Identity.Name;
                                valor.fechaCreacion = DateTime.Now;

                                switch (productoPropiedad.datoTipoid)
                                {
                                    case 1:
                                        valor.valorString = data["valor"].ToString();
                                        break;
                                    case 2:
                                        valor.valorEntero = Convert.ToInt32(data["valor"].ToString());
                                        break;
                                    case 3:
                                        valor.valorDecimal = Convert.ToDecimal(data["valor"].ToString());
                                        break;
                                    case 4:
                                        valor.valorEntero = data["valor"].ToString() == "true" ? 1 : 0;
                                        break;
                                    case 5:
                                        valor.valorTiempo = Convert.ToDateTime(data["valor_f"].ToString());
                                        break;
                                }
                                ret = (ret && ProductoPropiedadValorDAO.guardarProductoPropiedadValor(valor));
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = ret,
                        id = producto.id,
                        usuarioCreo = producto.usuarioCreo,
                        usuarioActualizo = producto.usuarioActualizo,
                        fechaCreacion = producto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = producto.fechaActualizacion != null ? producto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Productos - Eliminar")]
        public IActionResult Producto(int id)
        {
            try
            {
                Producto producto = ProductoDAO.getProductoPorId(id, User.Identity.Name);
                bool eliminado = ObjetoDAO.borrarHijos(producto.treepath, 3, User.Identity.Name);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Productos - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                int? componenteid = value.componenteid != null ? (int)value.componenteid : default(int);
                int? subcomponenteid = value.subcomponenteid != null ? (int)value.subcomponenteid : default(int);
                String filtro_busqueda = value.filtro_busqueda;

                long total = ProductoDAO.getTotalProductos(componenteid, subcomponenteid, filtro_busqueda, User.Identity.Name);

                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Productos - Visualizar")]
        public IActionResult ObtenerProductoPorId(int id)
        {
            try
            {
                Producto producto = ProductoDAO.getProductoPorId(id, User.Identity.Name);
                int congelado = 0;
                int prestamoId = 0;

                producto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.ejercicio, producto.entidad ?? default(int), producto.ueunidadEjecutora);
                if (producto.unidadEjecutoras != null)
                {
                    producto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.entidad ?? default(int), producto.ejercicio);
                }

                if (producto != null)
                {
                    Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(producto.treepath);
                    congelado = proyecto.congelado ?? default(int);
                    prestamoId = proyecto.prestamoid ?? default(int);
                }

                return Ok(new
                {
                    success = producto != null ? true : false,
                    id = producto.id,
                    prestamoId = prestamoId,
                    ejercicio = producto.ejercicio,
                    entidad = producto.entidad,
                    entidadNombre = producto.unidadEjecutoras != null ? producto.unidadEjecutoras.entidads.nombre : null,
                    unidadEjecutora = producto.ueunidadEjecutora,
                    unidadEjecutoraNombre = producto.unidadEjecutoras != null ? producto.unidadEjecutoras.nombre : null,
                    fechaInicio = producto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss"),
                    congelado = congelado,
                    nombre = producto != null ? producto.nombre : "Indefinido"
                });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Productos - Visualizar")]
        public IActionResult ProductoPorId(int id)
        {
            try
            {
                Producto producto = ProductoDAO.getProductoPorId(id, User.Identity.Name);
                Stproducto temp = new Stproducto();
                temp.id = producto.id;
                temp.nombre = producto.nombre;
                temp.descripcion = producto.descripcion;
                temp.programa = producto.programa;
                temp.subprograma = producto.subprograma;
                temp.proyecto = producto.proyecto;
                temp.obra = producto.obra;
                temp.actividad = producto.actividad;
                temp.renglon = producto.renglon;
                temp.ubicacionGeografica = producto.ubicacionGeografica;
                temp.duracion = producto.duracion;
                temp.duracionDimension = producto.duracionDimension;
                temp.fechaInicio = producto.fechaInicio != null ? producto.fechaInicio.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaFin = producto.fechaFin != null ? producto.fechaFin.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.snip = producto.snip;
                temp.estado = producto.estado ?? default(int);
                temp.usuarioCreo = producto.usuarioCreo;
                temp.usuarioActualizo = producto.usuarioActualizo;
                temp.fechaCreacion = producto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                temp.fechaActualizacion = producto.fechaActualizacion != null ? producto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.latitud = producto.latitud;
                temp.longitud = producto.longitud;
                temp.peso = producto.peso ?? default(int);
                temp.costo = producto.costo ?? default(decimal);

                producto.acumulacionCostos = AcumulacionCostoDAO.getAcumulacionCostoById(Convert.ToInt32(producto.acumulacionCostoid));

                temp.acumulacionCostoid = Convert.ToInt32(producto.acumulacionCostoid);
                temp.acumulacionCostoNombre = producto.acumulacionCostos != null ? producto.acumulacionCostos.nombre : null;

                producto.productoTipos = ProductoTipoDAO.getProductoTipo(producto.productoTipoid);
                temp.productoTipoid = producto.productoTipoid;
                temp.productoTipoNombre = producto.productoTipos.nombre;

                if (producto.componenteid != null)
                {
                    producto.componentes = ComponenteDAO.getComponentePorId(producto.componenteid ?? default(int), null);
                    temp.componenteid = producto.componenteid ?? default(int);
                    temp.componenteNombre = producto.componentes.nombre;

                    producto.componentes.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.componentes.ejercicio, producto.componentes.entidad ?? default(int), producto.componentes.ueunidadEjecutora);
                    if (producto.componentes.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = producto.componentes.ueunidadEjecutora;
                        temp.entidad = producto.componentes.entidad ?? default(int);
                        temp.ejercicio = producto.componentes.ejercicio;
                        temp.nombreUnidadEjecutora = producto.componentes.unidadEjecutoras.nombre;

                        producto.componentes.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.componentes.entidad ?? default(int), producto.componentes.ejercicio);
                        temp.entidadnombre = producto.componentes.unidadEjecutoras.entidads.nombre;
                    }
                }

                if (producto.subcomponenteid != null)
                {
                    producto.subcomponentes = SubComponenteDAO.getSubComponentePorId(producto.subcomponenteid ?? default(int), null);
                    temp.subcomponenteid = producto.subcomponenteid ?? default(int);
                    temp.subcomponenteNombre = producto.subcomponentes != null ? producto.subcomponentes.nombre : null;

                    producto.subcomponentes.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.subcomponentes.ejercicio ?? default(int), producto.subcomponentes.entidad ?? default(int), producto.subcomponentes.ueunidadEjecutora ?? default(int));
                    if (producto.subcomponentes.unidadEjecutoras != null)
                    {
                        temp.ueunidadEjecutora = producto.subcomponentes.ueunidadEjecutora ?? default(int);
                        temp.entidad = producto.subcomponentes.entidad ?? default(int);
                        temp.ejercicio = producto.subcomponentes.ejercicio ?? default(int);
                        temp.nombreUnidadEjecutora = producto.subcomponentes.unidadEjecutoras.nombre;

                        producto.subcomponentes.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.subcomponentes.entidad ?? default(int), producto.subcomponentes.ejercicio ?? default(int));
                        temp.entidadnombre = producto.subcomponentes.unidadEjecutoras.entidads.nombre;
                    }
                }

                producto.unidadEjecutoras = UnidadEjecutoraDAO.getUnidadEjecutora(producto.ejercicio, producto.entidad ?? default(int), producto.ueunidadEjecutora);
                if (producto.unidadEjecutoras != null)
                {
                    temp.ueunidadEjecutora = producto.ueunidadEjecutora;
                    temp.entidad = producto.entidad ?? default(int);
                    temp.ejercicio = producto.ejercicio;
                    temp.nombreUnidadEjecutora = producto.unidadEjecutoras.nombre;

                    producto.unidadEjecutoras.entidads = EntidadDAO.getEntidad(producto.entidad ?? default(int), producto.ejercicio);
                    temp.entidadnombre = producto.unidadEjecutoras.entidads.nombre;
                }

                temp.tieneHijos = ObjetoDAO.tieneHijos(temp.id, 3);
                temp.fechaInicioReal = producto.fechaInicioReal != null ? producto.fechaInicioReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaFinReal = producto.fechaFinReal != null ? producto.fechaFinReal.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                Proyecto proyecto = ProyectoDAO.getProyectobyTreePath(producto.treepath);
                temp.fechaElegibilidad = proyecto.fechaElegibilidad != null ? proyecto.fechaElegibilidad.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.fechaCierre = proyecto.fechaCierre != null ? proyecto.fechaCierre.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                temp.inversionNueva = producto.inversionNueva ?? default(int);

                return Ok(new { success = true, producto = temp });
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{proyectoId}")]
        [Authorize("Productos - Visualizar")]
        public IActionResult ProductoPorProyecto(int proyectoId)
        {
            try
            {
                List<Producto> productos = ProductoDAO.getProductosPorProyecto(proyectoId, User.Identity.Name, null);

                List<Stproducto> stproductos = new List<Stproducto>();

                foreach (Producto producto in productos)
                {
                    Stproducto temp = new Stproducto();
                    temp.id = producto.id;
                    temp.nombre = producto.nombre;
                    temp.descripcion = producto.descripcion;
                    temp.estado = producto.estado ?? default(int);
                    temp.usuarioCreo = producto.usuarioCreo;
                    temp.usuarioActualizo = producto.usuarioActualizo;
                    temp.fechaCreacion = producto.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = producto.fechaActualizacion != null ? producto.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.peso = producto.peso ?? default(int);
                    temp.inversionNueva = producto.inversionNueva ?? default(int);
                    stproductos.Add(temp);
                }

                return Ok(new { success = true, productos = stproductos });
            }
            catch (Exception e)
            {
                CLogger.write("8", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Productos - Editar")]
        public IActionResult PesoProducto([FromBody]dynamic value)
        {
            try
            {
                String param_productos = value.productos;
                String[] split = param_productos.Split("~");
                bool ret = true;
                for (int i = 0; i < split.Length; i++)
                {
                    String[] temp = split[i].Split(",");
                    if (temp.Length == 2)
                    {
                        Producto producto = ProductoDAO.getProductoPorId(Convert.ToInt32(temp[0]));
                        if (producto != null)
                        {
                            producto.peso = Convert.ToInt32(temp[1]);
                            producto.usuarioActualizo = User.Identity.Name;
                            producto.fechaActualizacion = DateTime.Now;
                            ret = ProductoDAO.guardarProducto(producto, true);
                        }
                    }
                }

                return Ok(new { success = ret });
            }
            catch (Exception e)
            {
                CLogger.write("9", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Productos - Visualizar")]
        public IActionResult CantidadHistoria(int id)
        {
            try
            {
                String resultado = ProductoDAO.getVersiones(id);
                return Ok(new { success = true, versiones = "[" + resultado + "]" });
                }
            catch (Exception e)
            {
                CLogger.write("10", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{id}/{version}")]
        [Authorize("Productos - Visualizar")]
        public IActionResult Historia(int id, int version)
        {
            try
            { 
                String resultado = ProductoDAO.getHistoria(id, version);
                return Ok(new { success = true, historia = resultado });
            }
            catch (Exception e)
            {
                CLogger.write("11", "ProductoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Productos - Visualizar")]
        public IActionResult ValidacionAsignado([FromBody]dynamic value)
        {
            try
            {
                DateTime cal = new DateTime();
                int ejercicio = cal.Year;
                Producto objProducto = ProductoDAO.getProductoPorId((int)value.id);
                Proyecto objProyecto = ProyectoDAO.getProyectobyTreePath(objProducto.treepath);

                int entidad = objProyecto.entidad ?? default(int);

                int programa = value.programa != null ? (int)value.programa : default(int);
                int subprograma = value.subprograma != null ? (int)value.subprograma : default(int);
                int proyecto = value.proyecto != null ? (int)value.proyecto : default(int);
                int actividad = value.actividad != null ? (int)value.actividad : default(int);
                int obra = value.obra != null ? (int)value.obra : default(int);
                int renglon = value.renglon != null ? (int)value.renglon : default(int);
                int geografico = value.geografico != null ? (int)value.geografico : default(int);
                decimal asignado = ObjetoDAO.getAsignadoPorLineaPresupuestaria(ejercicio, entidad, programa, subprograma, proyecto, actividad, obra, renglon, geografico);

                decimal planificado = decimal.Zero;
                switch (objProducto.acumulacionCostoid)
                {
                    case 1:
                        cal = objProducto.fechaInicio ?? default(DateTime);
                        int ejercicioInicial = cal.Year;
                        if (ejercicio.Equals(ejercicioInicial))
                        {
                            planificado = objProducto.costo ?? default(decimal);
                        }
                        break;
                    case 2:
                        List<PagoPlanificado> lstPagos = PagoPlanificadoDAO.getPagosPlanificadosPorObjeto(objProducto.id, 3);
                        foreach (PagoPlanificado pago in lstPagos)
                        {
                            cal = pago.fechaPago;
                            int ejercicioPago = cal.Year;
                            if (ejercicio.Equals(ejercicioPago))
                            {
                                planificado += pago.pago;
                            }
                        }
                        break;
                    case 3:
                        cal = objProducto.fechaFin ?? default(DateTime);
                        int ejercicioFinal = cal.Year;
                        if (ejercicio.Equals(ejercicioFinal))
                        {
                            planificado += objProducto.costo ?? default(decimal);
                        }
                        break;
                }

                bool sobrepaso = false;
                if ((asignado = (asignado - planificado)).CompareTo(decimal.Zero) == -1)
                    sobrepaso = true;

                return Ok(new { success = true, asignado = asignado, sobrepaso = sobrepaso });
            }
            catch (Exception e)
            {
                CLogger.write("11", "ProductoController.class", e);
                return BadRequest(500);
            }
        }        
    }
}
