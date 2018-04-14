
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the autorizacion_tipo table.
    /// </summary>
	[Table("autorizacion_tipo")]
	public partial class Autorizaciontipo
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
		public virtual IEnumerable<Autorizaciontipo> autorizaciontipoes { get; set; }
	}
}
