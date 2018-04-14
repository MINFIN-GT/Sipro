
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the estado table.
    /// </summary>
	[Table("estado")]
	public partial class Estado
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual int valor { get; set; }
		public virtual IEnumerable<Estado> estadoes { get; set; }
	}
}
