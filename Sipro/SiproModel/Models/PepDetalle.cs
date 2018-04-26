
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PEP_DETALLE table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PEP_DETALLE")]
	public partial class PepDetalle
	{
		[Key]
	    [ForeignKey("Proyecto")]
        public virtual Int32 proyectoid { get; set; }
	    public virtual string observaciones { get; set; }
	    public virtual string alertivos { get; set; }
	    public virtual string elaborado { get; set; }
	    public virtual string aprobado { get; set; }
	    public virtual string autoridad { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<PepDetalle> pepdetalles { get; set; }
	}
}
