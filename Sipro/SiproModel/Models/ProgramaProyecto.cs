
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the programa_proyecto table.
    /// </summary>
	[Table("programa_proyecto")]
	public partial class Programaproyecto
	{
		[Key]
	    [ForeignKey("programa")]
        public virtual int programaid { get; set; }
		[Key]
	    [ForeignKey("proyecto")]
        public virtual int proyectoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Programa tprograma { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual IEnumerable<Programaproyecto> programaproyectoes { get; set; }
	}
}
