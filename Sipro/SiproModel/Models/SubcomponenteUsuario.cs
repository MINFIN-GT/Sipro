
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the subcomponente_usuario table.
    /// </summary>
	[Table("subcomponente_usuario")]
	public partial class Subcomponenteusuario
	{
		[Key]
	    [ForeignKey("Subcomponente")]
        public virtual int subcomponenteid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Subcomponente tsubcomponente { get; set; }
		public virtual IEnumerable<Subcomponenteusuario> subcomponenteusuarios { get; set; }
	}
}
