
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the proyecto_usuario table.
    /// </summary>
	[Table("proyecto_usuario")]
	public partial class Proyectousuario
	{
		[Key]
	    [ForeignKey("Proyecto")]
        public virtual int proyectoid { get; set; }
		[Key]
	    [ForeignKey("Usuario")]
        public virtual string usuario { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual Usuario tusuario { get; set; }
		public virtual IEnumerable<Proyectousuario> proyectousuarios { get; set; }
	}
}
