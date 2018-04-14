
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the prestamo_usuario table.
    /// </summary>
	[Table("prestamo_usuario")]
	public partial class Prestamousuario
	{
		[Key]
	    [ForeignKey("prestamo")]
        public virtual int prestamoid { get; set; }
		[Key]
	    [ForeignKey("usuario")]
        public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Prestamo tprestamo { get; set; }
		public virtual Usuario tusuario { get; set; }
		public virtual IEnumerable<Prestamousuario> prestamousuarios { get; set; }
	}
}
