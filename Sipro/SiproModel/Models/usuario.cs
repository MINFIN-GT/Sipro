
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the usuario table.
    /// </summary>
	[Table("usuario")]
	public partial class Usuario
	{
		[Key]
	    public virtual string _usuario { get; set; }
	    public virtual string password { get; set; }
	    public virtual string salt { get; set; }
	    public virtual string email { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual int sistema_usuario { get; set; }
		public virtual IEnumerable<Usuario> usuarios { get; set; }
	}
}
