
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ACTIVIDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ACTIVIDAD")]
	public partial class Actividad
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [Column("FECHA_INICIO")]
	    public virtual DateTime fechaInicio { get; set; }
	    [Column("FECHA_FIN")]
	    public virtual DateTime fechaFin { get; set; }
	    [Column("PORCENTAJE_AVANCE")]
	    public virtual Int32 porcentajeAvance { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("ACTIVIDAD_TIPOID")]
	    [ForeignKey("ActividadTipo")]
        public virtual Int32 actividadTipoid { get; set; }
	    public virtual Int32? snip { get; set; }
	    public virtual Int32? programa { get; set; }
	    public virtual Int32? subprograma { get; set; }
	    public virtual Int32? proyecto { get; set; }
	    public virtual Int32? actividad { get; set; }
	    public virtual Int32? obra { get; set; }
	    [Column("OBJETO_ID")]
	    public virtual Int64 objetoId { get; set; }
	    [Column("OBJETO_TIPO")]
	    public virtual Int32 objetoTipo { get; set; }
	    public virtual Int32 duracion { get; set; }
	    [Column("DURACION_DIMENSION")]
	    public virtual string duracionDimension { get; set; }
	    [Column("PRED_OBJETO_ID")]
	    public virtual Int64? predObjetoId { get; set; }
	    [Column("PRED_OBJETO_TIPO")]
	    public virtual Int32? predObjetoTipo { get; set; }
	    public virtual string latitud { get; set; }
	    public virtual string longitud { get; set; }
	    public virtual decimal? costo { get; set; }
	    [Column("ACUMULACION_COSTO")]
	    [ForeignKey("AcumulacionCosto")]
        public virtual Int64 acumulacionCosto { get; set; }
	    public virtual Int32? renglon { get; set; }
	    [Column("UBICACION_GEOGRAFICA")]
	    public virtual Int32? ubicacionGeografica { get; set; }
	    public virtual Int32? orden { get; set; }
	    public virtual string treepath { get; set; }
	    public virtual Int32? nivel { get; set; }
	    [Column("PROYECTO_BASE")]
	    public virtual Int64? proyectoBase { get; set; }
	    [Column("COMPONENTE_BASE")]
	    public virtual Int32? componenteBase { get; set; }
	    [Column("PRODUCTO_BASE")]
	    public virtual Int32? productoBase { get; set; }
	    [Column("FECHA_INICIO_REAL")]
	    public virtual DateTime? fechaInicioReal { get; set; }
	    [Column("FECHA_FIN_REAL")]
	    public virtual DateTime? fechaFinReal { get; set; }
	    [Column("INVERSION_NUEVA")]
	    public virtual Int32 inversionNueva { get; set; }
		public virtual ActividadTipo actividadTipos { get; set; }
		public virtual AcumulacionCosto acumulacionCostos { get; set; }
		public virtual IEnumerable<Actividad> actividads { get; set; }
	}
}
