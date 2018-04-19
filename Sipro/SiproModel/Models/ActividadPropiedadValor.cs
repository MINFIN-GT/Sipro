
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ACTIVIDAD_PROPIEDAD_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ACTIVIDAD_PROPIEDAD_VALOR")]
	public partial class ActividadPropiedadValor
	{
		[Key]
	    [ForeignKey("Actividad")]
        public virtual string actividadid { get; set; }
		[Key]
	    [ForeignKey("ActividadPropiedad")]
        public virtual string actividadPropiedadid { get; set; }
	    public virtual string valorEntero { get; set; }
	    public virtual string valorString { get; set; }
	    public virtual string valorDecimal { get; set; }
	    public virtual string valorTiempo { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual ActividadPropiedad actividadPropiedads { get; set; }
		public virtual Actividad actividads { get; set; }
		public virtual IEnumerable<ActividadPropiedadValor> actividadpropiedadvalors { get; set; }
	}
}
