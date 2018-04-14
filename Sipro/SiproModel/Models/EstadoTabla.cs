
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the estado_tabla table.
    /// </summary>
	[Table("estado_tabla")]
	public partial class Estadotabla
	{
		[Key]
	    public virtual string usuario { get; set; }
		[Key]
	    public virtual string tabla { get; set; }
	    public virtual string valores { get; set; }
		public virtual IEnumerable<Estadotabla> estadotablas { get; set; }
	}
}
