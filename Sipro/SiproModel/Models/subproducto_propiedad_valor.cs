
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the SUBPRODUCTO_PROPIEDAD_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("subproducto_propiedad_valor")]
	public partial class subproducto_propiedad_valor
	{
		[Key]
	    [ForeignKey("subproducto")]
        public virtual string subproductoid { get; set; }
		[Key]
	    [ForeignKey("subproducto_propiedad")]
        public virtual string subproducto_propiedadid { get; set; }
	    public virtual string valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual string valor_decimal { get; set; }
	    public virtual string valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual subproducto_propiedad subproducto_propiedads { get; set; }
		public virtual subproducto subproductos { get; set; }
		public virtual IEnumerable<subproducto_propiedad_valor> subproductopropiedadvalors { get; set; }
	}
}
