
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the producto_usuario table.
    /// </summary>
	[Table("producto_usuario")]
	public partial class Productousuario
	{
		[Key]
	    [ForeignKey("Producto")]
        public virtual int productoid { get; set; }
		[Key]
	    [ForeignKey("Usuario")]
        public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Producto tproducto { get; set; }
		public virtual Usuario tusuario { get; set; }
		public virtual IEnumerable<Productousuario> productousuarios { get; set; }
	}
}
