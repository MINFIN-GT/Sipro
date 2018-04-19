
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ESTADO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ESTADO")]
	public partial class Estado
	{
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string valor { get; set; }
	}
}
