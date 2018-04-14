
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ejecucion_estado table.
    /// </summary>
	[Table("ejecucion_estado")]
	public partial class Ejecucionestado
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
		public virtual IEnumerable<Ejecucionestado> ejecucionestadoes { get; set; }
	}
}
