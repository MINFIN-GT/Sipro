
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the objeto_prestamo table.
    /// </summary>
	[Table("objeto_prestamo")]
	public partial class Objetoprestamo
	{
		[Key]
	    [ForeignKey("prestamo")]
        public virtual int prestamoid { get; set; }
		[Key]
	    public virtual int objeto_id { get; set; }
		[Key]
	    public virtual int objeto_tipo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual int? usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Prestamo tprestamo { get; set; }
		public virtual IEnumerable<Objetoprestamo> objetoprestamoes { get; set; }
	}
}
