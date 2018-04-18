
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the subproducto_usuario table.
    /// </summary>
	[Table("subproducto_usuario")]
	public partial class Subproductousuario
	{
		[Key]
	    [ForeignKey("Subproducto")]
        public virtual int subproductoid { get; set; }
		[Key]
	    [ForeignKey("Usuario")]
        public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Subproducto tsubproducto { get; set; }
		public virtual Usuario tusuario { get; set; }
		public virtual IEnumerable<Subproductousuario> subproductousuarios { get; set; }
	}
}
