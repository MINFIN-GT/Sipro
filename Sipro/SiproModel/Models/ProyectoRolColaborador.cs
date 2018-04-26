
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PROYECTO_ROL_COLABORADOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PROYECTO_ROL_COLABORADOR")]
	public partial class ProyectoRolColaborador
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    [ForeignKey("Colaborador")]
        public virtual Int32 colaboradorid { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual Int32 proyectoid { get; set; }
	    [Column("ROL_UNIDAD_EJECUTORAID")]
	    [ForeignKey("RolUnidadEjecutora")]
        public virtual Int32 rolUnidadEjecutoraid { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
		public virtual RolUnidadEjecutora rolUnidadEjecutoras { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual Colaborador colaboradors { get; set; }
		public virtual IEnumerable<ProyectoRolColaborador> proyectorolcolaboradors { get; set; }
	}
}
