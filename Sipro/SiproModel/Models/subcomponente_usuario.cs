
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the SUBCOMPONENTE_USUARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("subcomponente_usuario")]
	public partial class subcomponente_usuario
	{
		[Key]
	    [ForeignKey("subcomponente")]
        public virtual string subcomponenteid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
		public virtual subcomponente subcomponentes { get; set; }
		public virtual IEnumerable<subcomponente_usuario> subcomponenteusuarios { get; set; }
	}
}
