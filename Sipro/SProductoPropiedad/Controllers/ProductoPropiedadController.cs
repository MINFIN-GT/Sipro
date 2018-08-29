using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using Utilities;
using SiproModelCore.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace SProductoPropiedad.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProductoPropiedadController : Controller
    {
        private class StProductoPropiedad
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

        [HttpPost]
        [Authorize("Producto Propiedades - Visualizar")]
        public IActionResult ProductoPropiedadPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;

                List<ProductoPropiedad> lstProductoPropiedades = ProductoPropiedadDAO.getProductoPropiedadPagina(pagina, registros, filtro_busqueda, columna_ordenada, orden_direccion);

                List<StProductoPropiedad> listaEstructuraPojos = new List<StProductoPropiedad>();

                foreach (ProductoPropiedad productoPropiedad in lstProductoPropiedades)
                {
                    StProductoPropiedad temp = new StProductoPropiedad();
                    temp.id = productoPropiedad.id;
                    temp.nombre = productoPropiedad.nombre;
                    temp.descripcion = productoPropiedad.descripcion;

                    productoPropiedad.datoTipos = DatoTipoDAO.getDatoTipo(productoPropiedad.datoTipoid);

                    temp.datoTipoid = productoPropiedad.datoTipoid;
                    temp.datoTipoNombre = productoPropiedad.datoTipos.nombre;
                    temp.usuarioCreo = productoPropiedad.usuarioCreo;
                    temp.usuarioActualizo = productoPropiedad.usuarioActualizo;
                    temp.fechaCreacion = productoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = productoPropiedad.fechaActualizacion != null ? productoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;

                    listaEstructuraPojos.Add(temp);
                }

                return Ok(new { success = true, productoPropiedades= listaEstructuraPojos });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Producto Propiedades - Crear")]
        public IActionResult ProductoPropiedad([FromBody]dynamic value)
        {
            try
            {
                ProductoPropiedadValidator validator = new ProductoPropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int tipo = value.datoTipoid != null ? (int)value.datoTipoid : default(int);

                    ProductoPropiedad productoPropiedad = new ProductoPropiedad();
                    productoPropiedad.nombre = nombre;
                    productoPropiedad.descripcion = descripcion;
                    productoPropiedad.datoTipoid = tipo;
                    productoPropiedad.usuarioCreo = User.Identity.Name;
                    productoPropiedad.fechaCreacion = DateTime.Now;
                    productoPropiedad.estado = 1;

                    bool ret = ProductoPropiedadDAO.guardarProductoPropiedad(productoPropiedad);

                    return Ok(new
                    {
                        success = ret,
                        id = productoPropiedad.id,
                        usuarioCreo = productoPropiedad.usuarioCreo,
                        usuarioActualizo = productoPropiedad.usuarioActualizo,
                        fechaCreacion = productoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = productoPropiedad.fechaActualizacion != null ? productoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null,
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Producto Propiedades - Editar")]
        public IActionResult ProductoPropiedad(int id, [FromBody]dynamic value)
        {
            try
            {
                ProductoPropiedadValidator validator = new ProductoPropiedadValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    String nombre = value.nombre;
                    String descripcion = value.descripcion;
                    int tipo = value.datoTipoid != null ? (int)value.datoTipoid : default(int);

                    ProductoPropiedad productoPropiedad = ProductoPropiedadDAO.getProductoPropiedad(id);
                    productoPropiedad.nombre = nombre;
                    productoPropiedad.descripcion = descripcion;
                    productoPropiedad.datoTipoid = tipo;
                    productoPropiedad.usuarioActualizo = User.Identity.Name;
                    productoPropiedad.fechaActualizacion = DateTime.Now;

                    bool ret = ProductoPropiedadDAO.guardarProductoPropiedad(productoPropiedad);

                    return Ok(new
                    {
                        success = ret,
                        id = productoPropiedad.id,
                        usuarioCreo = productoPropiedad.usuarioCreo,
                        usuarioActualizo = productoPropiedad.usuarioActualizo,
                        fechaCreacion = productoPropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = productoPropiedad.fechaActualizacion != null ? productoPropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null,
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Producto Propiedades - Eliminar")]
        public IActionResult ProductoPropiedad(int id)
        {
            try
            {
                ProductoPropiedad productoPropiedad = ProductoPropiedadDAO.getProductoPropiedad(id);
                productoPropiedad.usuarioActualizo = User.Identity.Name;
                bool eliminado = ProductoPropiedadDAO.eliminar(productoPropiedad);

                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Producto Propiedades - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;
                long total = ProductoPropiedadDAO.getTotal(filtro_busqueda);

                return Ok(new { success =true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idproducto}/{idproductotipo}")]
        [Authorize("Producto Propiedades - Visualizar")]
        public IActionResult ProductoPropiedadPorTipo(int idproducto, int idproductotipo)
        {
            try
            {
                List<ProductoPropiedad> productoPropiedades = ProductoPropiedadDAO.getProductoPropiedadesPorTipo(idproductotipo);

                List<Dictionary<String, Object>> campos = new List<Dictionary<String, Object>>();
                foreach (ProductoPropiedad productoPropiedad in productoPropiedades)
                {
                    Dictionary<String, Object> campo = new Dictionary<String, Object>();
                    campo.Add("id", productoPropiedad.id);
                    campo.Add("nombre", productoPropiedad.nombre);
                    campo.Add("tipo", productoPropiedad.datoTipoid);
                    ProductoPropiedadValor productoPropiedadValor = ProductoPropiedadValorDAO.getValorPorProductoYPropiedad(productoPropiedad.id, idproducto);
                    if (productoPropiedadValor != null)
                    {
                        switch (productoPropiedad.datoTipoid)
                        {
                            case 1:
                                campo.Add("valor", productoPropiedadValor.valorString);
                                break;
                            case 2:
                                campo.Add("valor", productoPropiedadValor.valorEntero);
                                break;
                            case 3:
                                campo.Add("valor", productoPropiedadValor.valorDecimal);
                                break;
                            case 4:
                                campo.Add("valor", productoPropiedadValor.valorEntero == 1 ? true : false);
                                break;
                            case 5:
                                campo.Add("valor", productoPropiedadValor.valorTiempo != null ? productoPropiedadValor.valorTiempo.Value.ToString("dd/MM/yyyy H:mm:ss") : null);
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

                return Ok(new { success = true, productopropiedades = estructuraCamposDinamicos });
            }
            catch (Exception e)
            {
                CLogger.write("6", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }

        [HttpGet("{idProductoTipo}")]
        [Authorize("Producto Propiedades - Visualizar")]
        public IActionResult ProductoPropiedadPorTipoProducto(int idProductoTipo)
        {
            try
            {
                List<ProductoPropiedad> productopropiedades = ProductoPropiedadDAO.getProductoPropiedadesPorTipo(idProductoTipo);
                List<StProductoPropiedad> stproductopropiedad = new List<StProductoPropiedad>();
                foreach (ProductoPropiedad productopropiedad in productopropiedades)
                {
                    StProductoPropiedad temp = new StProductoPropiedad();
                    temp.id = productopropiedad.id;
                    temp.nombre = productopropiedad.nombre;
                    temp.descripcion = productopropiedad.descripcion;

                    productopropiedad.datoTipos = DatoTipoDAO.getDatoTipo(productopropiedad.datoTipoid);

                    temp.datoTipoid = productopropiedad.datoTipoid;
                    temp.datoTipoNombre = productopropiedad.datoTipos.nombre;
                    temp.fechaActualizacion = productopropiedad.fechaActualizacion != null ? productopropiedad.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.fechaCreacion = productopropiedad.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.usuarioActualizo = productopropiedad.usuarioActualizo;
                    temp.usuarioCreo = productopropiedad.usuarioCreo;
                    temp.estado = productopropiedad.estado;
                    stproductopropiedad.Add(temp);
                }

                return Ok(new { success = true, productopropiedades = stproductopropiedad });
            }
            catch (Exception e)
            {
                CLogger.write("7", "ProductoPropiedadController.class", e);
                return BadRequest(500);
            }
        }
    }
}
