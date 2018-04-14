
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the prestamo_tipo_prestamo table.
    /// </summary>
	[Table("prestamo_tipo_prestamo")]
	public partial class Prestamotipoprestamo
	{
		[Key]
	    [ForeignKey("prestamo")]
        public virtual int prestamoId { get; set; }
		[Key]
	    [ForeignKey("prestamotipo")]
        public virtual int tipoPrestamoId { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Prestamo tprestamo { get; set; }
		public virtual Prestamotipo tprestamotipo { get; set; }
		public virtual IEnumerable<Prestamotipoprestamo> prestamotipoprestamoes { get; set; }
	}
}
