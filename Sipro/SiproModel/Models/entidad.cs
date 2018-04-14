
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the entidad table.
    /// </summary>
	[Table("entidad")]
	public partial class Entidad
	{
		[Key]
	    public virtual int _entidad { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string abreviatura { get; set; }
		[Key]
	    public virtual int ejercicio { get; set; }
		public virtual IEnumerable<Entidad> entidads { get; set; }
	}
}
