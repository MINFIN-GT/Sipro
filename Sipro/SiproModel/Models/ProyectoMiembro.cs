
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the proyecto_miembro table.
    /// </summary>
	[Table("proyecto_miembro")]
	public partial class Proyectomiembro
	{
		[Key]
	    [ForeignKey("proyecto")]
        public virtual int proyectoid { get; set; }
		[Key]
	    [ForeignKey("colaborador")]
        public virtual int colaboradorid { get; set; }
	    public virtual int estado { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual Colaborador tcolaborador { get; set; }
		public virtual IEnumerable<Proyectomiembro> proyectomiembroes { get; set; }
	}
}
