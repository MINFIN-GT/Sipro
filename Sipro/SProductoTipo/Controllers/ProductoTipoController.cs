﻿using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SiproDAO.Dao;
using SiproModelCore.Models;
using Utilities;

namespace SProductoTipo.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowAllHeaders")]
    public class ProductoTipoController : Controller
    {
        private class Stproductotipo
        {
            public int id;
            public String nombre;
            public String descripcion;
            public String usuarioCreo;
            public String usuarioActualizo;
            public String fechaCreacion;
            public String fechaActualizacion;
            public int estado;
        }

        [HttpPost]
        [Authorize("Producto Tipos - Visualizar")]
        public IActionResult ProductoTipoPagina([FromBody]dynamic value)
        {
            try
            {
                int pagina = value.pagina != null ? (int)value.pagina : 1;
                int registros = value.registros != null ? (int)value.registros : 20;
                String filtro_busqueda = value.filtro_busqueda;
                String columna_ordenada = value.columna_ordenada;
                String orden_direccion = value.orden_direccion;


                List<ProductoTipo> productotipos = ProductoTipoDAO.getPagina(pagina, registros, filtro_busqueda, columna_ordenada, orden_direccion);
                List<Stproductotipo> listaProductoTipo = new List<Stproductotipo>();

                foreach (ProductoTipo productotipo in productotipos)
                {
                    Stproductotipo temp = new Stproductotipo();
                    temp.id = productotipo.id;
                    temp.nombre = productotipo.nombre;
                    temp.descripcion = productotipo.descripcion;
                    temp.usuarioCreo = productotipo.usuarioCreo;
                    temp.usuarioActualizo = productotipo.usuarioActualizo;
                    temp.fechaCreacion = productotipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss");
                    temp.fechaActualizacion = productotipo.fechaActualizacion != null ? productotipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null;
                    temp.estado = productotipo.estado;

                    listaProductoTipo.Add(temp);
                }

                return Ok(new { success = true, productoTipos = listaProductoTipo });
            }
            catch (Exception e)
            {
                CLogger.write("1", "ProductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Producto Tipos - Crear")]
        public IActionResult ProductoTipo([FromBody]dynamic value)
        {
            try
            {
                ProductoTipoValidator validator = new ProductoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ProductoTipo productoTipo = new ProductoTipo();
                    productoTipo.nombre = value.nombre;
                    productoTipo.descripcion = value.descripcion;
                    productoTipo.usuarioCreo = User.Identity.Name;
                    productoTipo.fechaCreacion = DateTime.Now;
                    productoTipo.estado = 1;

                    bool guardado = false;
                    guardado = ProductoTipoDAO.guardarProductoTipo(productoTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                ProdtipoPropiedad prodtipoPropiedad = new ProdtipoPropiedad();
                                prodtipoPropiedad.productoTipoid = productoTipo.id;
                                prodtipoPropiedad.productoPropiedadid = Convert.ToInt32(idPropiedad);
                                prodtipoPropiedad.fechaCreacion = DateTime.Now;
                                prodtipoPropiedad.usuarioCreo = User.Identity.Name;

                                guardado = guardado & ProdTipoPropiedadDAO.guardarProdTipoPropiedad(prodtipoPropiedad);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = guardado,
                        id = productoTipo.id,
                        usuarioCreo = productoTipo.usuarioCreo,
                        usuarioActualizo = productoTipo.usuarioActualizo,
                        fechaCreacion = productoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = productoTipo.fechaActualizacion != null ? productoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("2", "ProductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Producto Tipos - Editar")]
        public IActionResult ProductoTipo(int id, [FromBody]dynamic value)
        {
            try
            {
                ProductoTipoValidator validator = new ProductoTipoValidator();
                ValidationResult results = validator.Validate(value);

                if (results.IsValid)
                {
                    ProductoTipo productoTipo = ProductoTipoDAO.getProductoTipo(id);
                    productoTipo.nombre = value.nombre;
                    productoTipo.descripcion = value.descripcion;
                    productoTipo.usuarioActualizo = User.Identity.Name;
                    productoTipo.fechaActualizacion = DateTime.Now;

                    bool guardado = false;
                    guardado = ProductoTipoDAO.guardarProductoTipo(productoTipo);

                    if (guardado)
                    {
                        string propiedades = value.propiedades != null ? (string)value.propiedades : default(string);
                        String[] idsPropiedades = propiedades != null && propiedades.Length > 0 ? propiedades.Split(",") : null;

                        if (idsPropiedades != null && idsPropiedades.Length > 0)
                        {
                            foreach (String idPropiedad in idsPropiedades)
                            {
                                ProdtipoPropiedad prodtipoPropiedad = new ProdtipoPropiedad();
                                prodtipoPropiedad.productoTipoid = productoTipo.id;
                                prodtipoPropiedad.productoPropiedadid = Convert.ToInt32(idPropiedad);
                                prodtipoPropiedad.fechaCreacion = DateTime.Now;
                                prodtipoPropiedad.usuarioCreo = User.Identity.Name;

                                guardado = guardado & ProdTipoPropiedadDAO.guardarProdTipoPropiedad(prodtipoPropiedad);
                            }
                        }
                    }

                    return Ok(new
                    {
                        success = guardado,
                        id = productoTipo.id,
                        usuarioCreo = productoTipo.usuarioCreo,
                        usuarioActualizo = productoTipo.usuarioActualizo,
                        fechaCreacion = productoTipo.fechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                        fechaActualizacion = productoTipo.fechaActualizacion != null ? productoTipo.fechaActualizacion.Value.ToString("dd/MM/yyyy H:mm:ss") : null
                    });
                }
                else
                    return Ok(new { success = false });
            }
            catch (Exception e)
            {
                CLogger.write("3", "ProductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Producto Tipos - Eliminar")]
        public IActionResult ProductoTipo(int id)
        {
            try
            {
                ProductoTipo productoTipo = ProductoTipoDAO.getProductoTipo(id);
                productoTipo.usuarioActualizo = User.Identity.Name;
                bool eliminado = ProductoTipoDAO.eliminarProductoTipo(productoTipo);
                return Ok(new { success = eliminado });
            }
            catch (Exception e)
            {
                CLogger.write("4", "ProductoTipoController.class", e);
                return BadRequest(500);
            }
        }

        [HttpPost]
        [Authorize("Producto Tipos - Visualizar")]
        public IActionResult TotalElementos([FromBody]dynamic value)
        {
            try
            {
                String filtro_busqueda = value.filtro_busqueda;
                long total = ProductoTipoDAO.getTotal(filtro_busqueda);

                return Ok(new { success = true, total = total });
            }
            catch (Exception e)
            {
                CLogger.write("5", "ProductoTipoController.class", e);
                return BadRequest(500);
            }
        }
    }
}
