
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the USUARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("usuario")]
	public partial class usuario
	{
		[Key]
	    public virtual string _usuario { get; set; }
	    public virtual string password { get; set; }
	    public virtual string salt { get; set; }
	    public virtual string email { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string sistema_usuario { get; set; }
	}
}
