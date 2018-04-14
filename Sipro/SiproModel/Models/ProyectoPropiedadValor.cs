
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the proyecto_propiedad_valor table.
    /// </summary>
	[Table("proyecto_propiedad_valor")]
	public partial class Proyectopropiedadvalor
	{
		[Key]
	    [ForeignKey("proyecto")]
        public virtual int proyectoid { get; set; }
		[Key]
	    [ForeignKey("proyectopropiedad")]
        public virtual int proyecto_propiedadid { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Proyectopropiedad tproyectopropiedad { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual IEnumerable<Proyectopropiedadvalor> proyectopropiedadvalors { get; set; }
	}
}
