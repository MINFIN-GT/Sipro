
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the SUBPRODUCTO_USUARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("subproducto_usuario")]
	public partial class subproducto_usuario
	{
		[Key]
	    [ForeignKey("subproducto")]
        public virtual string subproductoid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
		public virtual subproducto subproductos { get; set; }
		public virtual IEnumerable<subproducto_usuario> subproductousuarios { get; set; }
	}
}
