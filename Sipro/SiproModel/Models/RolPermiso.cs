
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the rol_permiso table.
    /// </summary>
	[Table("rol_permiso")]
	public partial class Rolpermiso
	{
		[Key]
	    [ForeignKey("rol")]
        public virtual int rolid { get; set; }
		[Key]
	    [ForeignKey("permiso")]
        public virtual int permisoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Permiso tpermiso { get; set; }
		public virtual Rol trol { get; set; }
		public virtual IEnumerable<Rolpermiso> rolpermisoes { get; set; }
	}
}
