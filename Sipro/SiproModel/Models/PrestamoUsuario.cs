
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PRESTAMO_USUARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PRESTAMO_USUARIO")]
	public partial class PrestamoUsuario
	{
		[Key]
	    [ForeignKey("Prestamo")]
        public virtual Int32 prestamoid { get; set; }
		[Key]
	    [ForeignKey("Usuario")]
        public virtual string usuario { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime? fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		public virtual Usuario usuarios { get; set; }
		public virtual Prestamo prestamos { get; set; }
		public virtual IEnumerable<PrestamoUsuario> prestamousuarios { get; set; }
	}
}
