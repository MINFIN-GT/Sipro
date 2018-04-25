
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PROYECTO_MIEMBRO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PROYECTO_MIEMBRO")]
	public partial class ProyectoMiembro
	{
		[Key]
	    [ForeignKey("Proyecto")]
        public virtual string proyectoid { get; set; }
		[Key]
	    [ForeignKey("Colaborador")]
        public virtual string colaboradorid { get; set; }
	    public virtual string estado { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
		public virtual Colaborador colaboradors { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<ProyectoMiembro> proyectomiembroes { get; set; }
	}
}
