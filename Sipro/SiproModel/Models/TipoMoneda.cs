
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the TIPO_MONEDA table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("TIPO_MONEDA")]
	public partial class TipoMoneda
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string simbolo { get; set; }
	}
}
