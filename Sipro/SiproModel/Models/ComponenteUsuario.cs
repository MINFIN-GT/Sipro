
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the componente_usuario table.
    /// </summary>
	[Table("componente_usuario")]
	public partial class Componenteusuario
	{
		[Key]
	    [ForeignKey("componente")]
        public virtual int componenteid { get; set; }
		[Key]
	    [ForeignKey("usuario")]
        public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Componente tcomponente { get; set; }
		public virtual Usuario tusuario { get; set; }
		public virtual IEnumerable<Componenteusuario> componenteusuarios { get; set; }
	}
}
