
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ESTADO_TABLA table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ESTADO_TABLA")]
	public partial class EstadoTabla
	{
		[Key]
	    public virtual string usuario { get; set; }
		[Key]
	    public virtual string tabla { get; set; }
	    public virtual string valores { get; set; }
	}
}
