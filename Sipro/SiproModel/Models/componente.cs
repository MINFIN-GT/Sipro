
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
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual string proyectoid { get; set; }
	    [Column("COMPONENTE_TIPOID")]
	    [ForeignKey("ComponenteTipo")]
        public virtual string componenteTipoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    [Column("UEUNIDAD_EJECUTORA")]
	    [ForeignKey("UnidadEjecutora")]
        public virtual string ueunidadEjecutora { get; set; }
	    public virtual string snip { get; set; }
	    public virtual string programa { get; set; }
	    public virtual string subprograma { get; set; }
	    public virtual string proyecto { get; set; }
	    public virtual string actividad { get; set; }
	    public virtual string obra { get; set; }
	    public virtual string latitud { get; set; }
	    public virtual string longitud { get; set; }
	    public virtual string costo { get; set; }
	    [Column("ACUMULACION_COSTOID")]
	    [ForeignKey("AcumulacionCosto")]
        public virtual string acumulacionCostoid { get; set; }
	    public virtual string renglon { get; set; }
	    [Column("UBICACION_GEOGRAFICA")]
	    public virtual string ubicacionGeografica { get; set; }
	    [Column("FECHA_INICIO")]
	    public virtual string fechaInicio { get; set; }
	    [Column("FECHA_FIN")]
	    public virtual string fechaFin { get; set; }
	    public virtual string duracion { get; set; }
	    [Column("DURACION_DIMENSION")]
	    public virtual string duracionDimension { get; set; }
	    public virtual string orden { get; set; }
	    public virtual string treepath { get; set; }
	    public virtual string nivel { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string ejercicio { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string entidad { get; set; }
	    [Column("ES_DE_SIGADE")]
	    public virtual string esDeSigade { get; set; }
	    [Column("FUENTE_PRESTAMO")]
	    public virtual string fuentePrestamo { get; set; }
	    [Column("FUENTE_DONACION")]
	    public virtual string fuenteDonacion { get; set; }
	    [Column("FUENTE_NACIONAL")]
	    public virtual string fuenteNacional { get; set; }
	    [Column("COMPONENTE_SIGADEID")]
	    [ForeignKey("ComponenteSigade")]
        public virtual string componenteSigadeid { get; set; }
	    [Column("FECHA_INICIO_REAL")]
	    public virtual string fechaInicioReal { get; set; }
	    [Column("FECHA_FIN_REAL")]
	    public virtual string fechaFinReal { get; set; }
	    [Column("INVERSION_NUEVA")]
	    public virtual string inversionNueva { get; set; }
		public virtual ComponenteSigade componenteSigades { get; set; }
		public virtual AcumulacionCosto acumulacionCostos { get; set; }
		public virtual UnidadEjecutora unidadEjecutoras { get; set; }
		public virtual ComponenteTipo componenteTipos { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<Componente> componentes { get; set; }
	}
}
