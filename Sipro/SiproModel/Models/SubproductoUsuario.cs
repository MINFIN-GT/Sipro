
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
	[Table("SUBPRODUCTO_USUARIO")]
	public partial class SubproductoUsuario
	{
		[Key]
	    [ForeignKey("Subproducto")]
        public virtual string subproductoid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
		public virtual Subproducto subproductos { get; set; }
		public virtual IEnumerable<SubproductoUsuario> subproductousuarios { get; set; }
	}
}
