
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the riesgo_propiedad_valor table.
    /// </summary>
	[Table("riesgo_propiedad_valor")]
	public partial class Riesgopropiedadvalor
	{
		[Key]
	    [ForeignKey("riesgo")]
        public virtual int riesgoid { get; set; }
		[Key]
	    [ForeignKey("riesgopropiedad")]
        public virtual int riesgo_propiedadid { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Riesgo triesgo { get; set; }
		public virtual Riesgopropiedad triesgopropiedad { get; set; }
		public virtual IEnumerable<Riesgopropiedadvalor> riesgopropiedadvalors { get; set; }
	}
}
