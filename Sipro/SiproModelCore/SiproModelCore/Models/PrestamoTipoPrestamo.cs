
namespace SiproModelCore.Models
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
        public virtual Int32 prestamoid { get; set; }
	    [ForeignKey("PrestamoTipo")]
        public virtual Int64 tipoprestamoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
		public virtual Prestamo prestamos { get; set; }
		public virtual PrestamoTipo prestamoTipos { get; set; }
		public virtual IEnumerable<PrestamoTipoPrestamo> prestamotipoprestamoes { get; set; }
	}
}
