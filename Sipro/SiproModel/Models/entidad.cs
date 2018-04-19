
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ENTIDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ENTIDAD")]
	public partial class Entidad
	{
		[Key]
	    public virtual string entidad { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string abreviatura { get; set; }
	    public virtual string ejercicio { get; set; }
	}
}
