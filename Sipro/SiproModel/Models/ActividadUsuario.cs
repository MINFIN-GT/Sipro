
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the actividad_usuario table.
    /// </summary>
	[Table("actividad_usuario")]
	public partial class Actividadusuario
	{
		[Key]
	    [ForeignKey("actividad")]
        public virtual int actividadid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Actividad tactividad { get; set; }
		public virtual IEnumerable<Actividadusuario> actividadusuarios { get; set; }
	}
}
