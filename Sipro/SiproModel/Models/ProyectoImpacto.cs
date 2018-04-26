
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PROYECTO_IMPACTO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PROYECTO_IMPACTO")]
	public partial class ProyectoImpacto
	{
		[Key]
	    public virtual Int64 id { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual Int32 proyectoid { get; set; }
	    [ForeignKey("Entidad")]
        public virtual Int32 entidadentidad { get; set; }
	    public virtual string impacto { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
	    public virtual Int32 ejercicio { get; set; }
		public virtual Entidad entidads { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<ProyectoImpacto> proyectoimpactoes { get; set; }
	}
}
