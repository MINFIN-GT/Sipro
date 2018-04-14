
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the usuario_log table.
    /// </summary>
	[Table("usuario_log")]
	public partial class Usuariolog
	{
		[Key]
	    public virtual string usuario { get; set; }
		[Key]
	    public virtual byte[] fecha { get; set; }
		public virtual IEnumerable<Usuariolog> usuariologs { get; set; }
	}
}
