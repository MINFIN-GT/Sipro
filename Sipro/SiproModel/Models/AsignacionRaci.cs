
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
	    public virtual Int64 id { get; set; }
	    [Column("MATRIZ_RACIID")]
	    [ForeignKey("MatrizRaci")]
        public virtual Int64 matrizRaciid { get; set; }
	    [ForeignKey("Colaborador")]
        public virtual Int32 colaboradorid { get; set; }
	    [Column("ROL_RACI")]
	    public virtual string rolRaci { get; set; }
	    [Column("OBJETO_ID")]
	    public virtual Int64 objetoId { get; set; }
	    [Column("OBJETO_TIPO")]
	    public virtual Int64 objetoTipo { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
		public virtual Colaborador colaboradors { get; set; }
		public virtual MatrizRaci matrizRacis { get; set; }
		public virtual IEnumerable<AsignacionRaci> asignacionracis { get; set; }
	}
}
