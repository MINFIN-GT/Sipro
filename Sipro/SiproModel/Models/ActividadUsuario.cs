
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ACTIVIDAD_USUARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ACTIVIDAD_USUARIO")]
	public partial class ActividadUsuario
	{
		[Key]
	    [ForeignKey("Actividad")]
        public virtual Int32 actividadid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
		public virtual Actividad actividads { get; set; }
		public virtual IEnumerable<ActividadUsuario> actividadusuarios { get; set; }
	}
}
