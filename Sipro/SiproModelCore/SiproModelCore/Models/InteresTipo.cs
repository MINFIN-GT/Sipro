
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the INTERES_TIPO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("INTERES_TIPO")]
	public partial class InteresTipo
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	}
}