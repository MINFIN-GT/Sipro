
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
        public virtual Int32 proyectoid { get; set; }
		[Key]
	    [ForeignKey("Colaborador")]
        public virtual Int32 colaboradorid { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
<<<<<<< Updated upstream
		public virtual Proyecto proyectos { get; set; }
		public virtual Colaborador colaboradors { get; set; }
=======
		public virtual Colaborador colaboradors { get; set; }
		public virtual Proyecto proyectos { get; set; }
>>>>>>> Stashed changes
		public virtual IEnumerable<ProyectoMiembro> proyectomiembroes { get; set; }
	}
}
