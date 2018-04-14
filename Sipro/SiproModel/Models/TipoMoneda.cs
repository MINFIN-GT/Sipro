
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the tipo_moneda table.
    /// </summary>
	[Table("tipo_moneda")]
	public partial class Tipomoneda
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string simbolo { get; set; }
		public virtual IEnumerable<Tipomoneda> tipomonedas { get; set; }
	}
}
