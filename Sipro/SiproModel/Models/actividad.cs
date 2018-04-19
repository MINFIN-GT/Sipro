
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
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string fechaInicio { get; set; }
	    public virtual string fechaFin { get; set; }
	    public virtual string porcentajeAvance { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    [ForeignKey("ActividadTipo")]
        public virtual string actividadTipoid { get; set; }
	    public virtual string snip { get; set; }
	    public virtual string programa { get; set; }
	    public virtual string subprograma { get; set; }
	    public virtual string proyecto { get; set; }
	    public virtual string actividad { get; set; }
	    public virtual string obra { get; set; }
	    public virtual string objetoId { get; set; }
	    public virtual string objetoTipo { get; set; }
	    public virtual string duracion { get; set; }
	    public virtual string duracionDimension { get; set; }
	    public virtual string predObjetoId { get; set; }
	    public virtual string predObjetoTipo { get; set; }
	    public virtual string latitud { get; set; }
	    public virtual string longitud { get; set; }
	    public virtual string costo { get; set; }
	    [ForeignKey("AcumulacionCosto")]
        public virtual string acumulacionCosto { get; set; }
	    public virtual string renglon { get; set; }
	    public virtual string ubicacionGeografica { get; set; }
	    public virtual string orden { get; set; }
	    public virtual string treepath { get; set; }
	    public virtual string nivel { get; set; }
	    public virtual string proyectoBase { get; set; }
	    public virtual string componenteBase { get; set; }
	    public virtual string productoBase { get; set; }
	    public virtual string fechaInicioReal { get; set; }
	    public virtual string fechaFinReal { get; set; }
	    public virtual string inversionNueva { get; set; }
		public virtual AcumulacionCosto acumulacionCostos { get; set; }
		public virtual ActividadTipo actividadTipos { get; set; }
		public virtual IEnumerable<Actividad> actividads { get; set; }
	}
}
