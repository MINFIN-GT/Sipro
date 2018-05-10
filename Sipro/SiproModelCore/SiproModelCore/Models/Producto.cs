
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PRODUCTO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PRODUCTO")]
	public partial class Producto
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [ForeignKey("Componente")]
        public virtual Int32? componenteid { get; set; }
	    [ForeignKey("Subcomponente")]
        public virtual Int32 subcomponenteid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    [Column("PRODUCTO_TIPOID")]
	    [ForeignKey("ProductoTipo")]
        public virtual Int32 productoTipoid { get; set; }
	    public virtual Int32? estado { get; set; }
	    [Column("UEUNIDAD_EJECUTORA")]
	    [ForeignKey("UnidadEjecutora")]
        public virtual Int32 ueunidadEjecutora { get; set; }
	    public virtual Int32? snip { get; set; }
	    public virtual Int32? programa { get; set; }
	    public virtual Int32? subprograma { get; set; }
	    public virtual Int32? proyecto { get; set; }
	    public virtual Int32? actividad { get; set; }
	    public virtual Int32? obra { get; set; }
	    public virtual string latitud { get; set; }
	    public virtual string longitud { get; set; }
	    public virtual Int32? peso { get; set; }
	    public virtual decimal? costo { get; set; }
	    [Column("ACUMULACION_COSTOID")]
	    [ForeignKey("AcumulacionCosto")]
        public virtual Int64 acumulacionCostoid { get; set; }
	    public virtual Int32? renglon { get; set; }
	    [Column("UBICACION_GEOGRAFICA")]
	    public virtual Int32? ubicacionGeografica { get; set; }
	    [Column("FECHA_INICIO")]
	    public virtual DateTime? fechaInicio { get; set; }
	    [Column("FECHA_FIN")]
	    public virtual DateTime? fechaFin { get; set; }
	    public virtual Int32 duracion { get; set; }
	    [Column("DURACION_DIMENSION")]
	    public virtual string duracionDimension { get; set; }
	    public virtual Int32? orden { get; set; }
	    public virtual string treepath { get; set; }
	    public virtual Int32? nivel { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual Int32 ejercicio { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual Int32? entidad { get; set; }
	    [Column("FECHA_INICIO_REAL")]
	    public virtual DateTime? fechaInicioReal { get; set; }
	    [Column("FECHA_FIN_REAL")]
	    public virtual DateTime? fechaFinReal { get; set; }
	    [Column("INVERSION_NUEVA")]
	    public virtual Int32? inversionNueva { get; set; }
		public virtual Componente componentes { get; set; }
		public virtual ProductoTipo productoTipos { get; set; }
		public virtual UnidadEjecutora unidadEjecutoras { get; set; }
		public virtual Subcomponente subcomponentes { get; set; }
		public virtual AcumulacionCosto acumulacionCostos { get; set; }
		public virtual IEnumerable<Producto> productoes { get; set; }
	}
}
