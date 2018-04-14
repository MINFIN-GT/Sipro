
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the subproducto_propiedad_valor table.
    /// </summary>
	[Table("subproducto_propiedad_valor")]
	public partial class Subproductopropiedadvalor
	{
		[Key]
	    [ForeignKey("subproducto")]
        public virtual int subproductoid { get; set; }
		[Key]
	    [ForeignKey("subproductopropiedad")]
        public virtual int subproducto_propiedadid { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Subproductopropiedad tsubproductopropiedad { get; set; }
		public virtual Subproducto tsubproducto { get; set; }
		public virtual IEnumerable<Subproductopropiedadvalor> subproductopropiedadvalors { get; set; }
	}
}
