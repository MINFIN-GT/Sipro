
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the interes_tipo table.
    /// </summary>
	[Table("interes_tipo")]
	public partial class Interestipo
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
		public virtual IEnumerable<Interestipo> interestipoes { get; set; }
	}
}
