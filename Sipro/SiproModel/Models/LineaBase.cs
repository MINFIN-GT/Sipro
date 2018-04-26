
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the LINEA_BASE table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("LINEA_BASE")]
	public partial class LineaBase
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual Int32 proyectoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
	    [Column("TIPO_LINEA_BASE")]
	    public virtual Int32 tipoLineaBase { get; set; }
	    public virtual Int32? sobreescribir { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<LineaBase> lineabases { get; set; }
	}
}
