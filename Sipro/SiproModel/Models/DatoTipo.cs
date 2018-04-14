
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the dato_tipo table.
    /// </summary>
	[Table("dato_tipo")]
	public partial class Datotipo
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
		public virtual IEnumerable<Datotipo> datotipoes { get; set; }
	}
}
