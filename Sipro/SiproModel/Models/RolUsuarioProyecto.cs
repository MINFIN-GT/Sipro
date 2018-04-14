
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the rol_usuario_proyecto table.
    /// </summary>
	[Table("rol_usuario_proyecto")]
	public partial class Rolusuarioproyecto
	{
		[Key]
	    public virtual int rol { get; set; }
		[Key]
	    public virtual int proyecto { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
		public virtual IEnumerable<Rolusuarioproyecto> rolusuarioproyectoes { get; set; }
	}
}
