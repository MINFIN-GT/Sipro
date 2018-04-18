
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the actividad_propiedad_valor table.
    /// </summary>
	[Table("actividad_propiedad_valor")]
	public partial class Actividadpropiedadvalor
	{
		[Key]
	    [ForeignKey("Actividad")]
        public virtual int actividadid { get; set; }
		[Key]
	    [ForeignKey("Actividadpropiedad")]
        public virtual int actividad_propiedadid { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Actividadpropiedad tactividadpropiedad { get; set; }
		public virtual Actividad tactividad { get; set; }
		public virtual IEnumerable<Actividadpropiedadvalor> actividadpropiedadvalors { get; set; }
	}
}
