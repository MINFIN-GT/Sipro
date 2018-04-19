
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ASIGNACION_RACI table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ASIGNACION_RACI")]
	public partial class AsignacionRaci
	{
		[Key]
	    public virtual string id { get; set; }
	    [ForeignKey("MatrizRaci")]
        public virtual string matrizRaciid { get; set; }
	    [ForeignKey("Colaborador")]
        public virtual string colaboradorid { get; set; }
	    public virtual string rolRaci { get; set; }
	    public virtual string objetoId { get; set; }
	    public virtual string objetoTipo { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual Colaborador colaboradors { get; set; }
		public virtual MatrizRaci matrizRacis { get; set; }
		public virtual IEnumerable<AsignacionRaci> asignacionracis { get; set; }
	}
}
