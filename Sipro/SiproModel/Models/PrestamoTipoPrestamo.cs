
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PRESTAMO_TIPO_PRESTAMO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PRESTAMO_TIPO_PRESTAMO")]
	public partial class PrestamoTipoPrestamo
	{
	    [ForeignKey("Prestamo")]
        public virtual string prestamoid { get; set; }
	    [ForeignKey("PrestamoTipo")]
        public virtual string tipoPrestamoid { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual PrestamoTipo prestamoTipos { get; set; }
		public virtual Prestamo prestamos { get; set; }
		public virtual IEnumerable<PrestamoTipoPrestamo> prestamotipoprestamoes { get; set; }
	}
}
