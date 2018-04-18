
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the asignacion_raci table.
    /// </summary>
	[Table("asignacion_raci")]
	public partial class Asignacionraci
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("Colaborador")]
        public virtual int colaboradorid { get; set; }
	    public virtual string rol_raci { get; set; }
	    public virtual int objeto_id { get; set; }
	    public virtual int objeto_tipo { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Colaborador tcolaborador { get; set; }
		public virtual IEnumerable<Asignacionraci> asignacionracis { get; set; }
	}
}
