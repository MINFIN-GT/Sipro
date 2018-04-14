
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the pep_detalle table.
    /// </summary>
	[Table("pep_detalle")]
	public partial class Pepdetalle
	{
		[Key]
	    [ForeignKey("proyecto")]
        public virtual int proyectoid { get; set; }
	    public virtual string observaciones { get; set; }
	    public virtual string alertivos { get; set; }
	    public virtual string elaborado { get; set; }
	    public virtual string aprobado { get; set; }
	    public virtual string autoridad { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual IEnumerable<Pepdetalle> pepdetalles { get; set; }
	}
}
