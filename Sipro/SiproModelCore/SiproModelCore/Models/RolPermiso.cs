
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ROL_PERMISO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ROL_PERMISO")]
	public partial class RolPermiso
	{
		[Key]
	    [ForeignKey("Rol")]
        public virtual Int32 rolid { get; set; }
		[Key]
	    [ForeignKey("Permiso")]
        public virtual Int64 permisoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
		public virtual Rol rols { get; set; }
		public virtual Permiso permisos { get; set; }
		public virtual IEnumerable<RolPermiso> rolpermisoes { get; set; }
	}
}
