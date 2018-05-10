
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the COMPONENTE table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("COMPONENTE")]
	public partial class Componente
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual Int32 proyectoid { get; set; }
	    [Column("COMPONENTE_TIPOID")]
	    [ForeignKey("ComponenteTipo")]
        public virtual Int32 componenteTipoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
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
	    public virtual decimal? costo { get; set; }
	    [Column("ACUMULACION_COSTOID")]
	    [ForeignKey("AcumulacionCosto")]
        public virtual Int64? acumulacionCostoid { get; set; }
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
	    [Column("ES_DE_SIGADE")]
	    public virtual Int32? esDeSigade { get; set; }
	    [Column("FUENTE_PRESTAMO")]
	    public virtual decimal? fuentePrestamo { get; set; }
	    [Column("FUENTE_DONACION")]
	    public virtual decimal? fuenteDonacion { get; set; }
	    [Column("FUENTE_NACIONAL")]
	    public virtual decimal? fuenteNacional { get; set; }
	    [Column("COMPONENTE_SIGADEID")]
	    [ForeignKey("ComponenteSigade")]
        public virtual Int32 componenteSigadeid { get; set; }
	    [Column("FECHA_INICIO_REAL")]
	    public virtual DateTime? fechaInicioReal { get; set; }
	    [Column("FECHA_FIN_REAL")]
	    public virtual DateTime? fechaFinReal { get; set; }
	    [Column("INVERSION_NUEVA")]
	    public virtual Int32 inversionNueva { get; set; }
		public virtual ComponenteSigade componenteSigades { get; set; }
		public virtual AcumulacionCosto acumulacionCostos { get; set; }
		public virtual UnidadEjecutora unidadEjecutoras { get; set; }
		public virtual ComponenteTipo componenteTipos { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<Componente> componentes { get; set; }
	}
}
