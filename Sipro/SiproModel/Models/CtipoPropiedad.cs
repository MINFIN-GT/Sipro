
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the CTIPO_PROPIEDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("CTIPO_PROPIEDAD")]
	public partial class CtipoPropiedad
	{
		[Key]
	    [Column("COMPONENTE_TIPOID")]
	    [ForeignKey("ComponenteTipo")]
        public virtual Int32 componenteTipoid { get; set; }
		[Key]
	    [Column("COMPONENTE_PROPIEDADID")]
	    [ForeignKey("ComponentePropiedad")]
        public virtual Int32 componentePropiedadid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
		public virtual ComponentePropiedad componentePropiedads { get; set; }
		public virtual ComponenteTipo componenteTipos { get; set; }
		public virtual IEnumerable<CtipoPropiedad> ctipopropiedads { get; set; }
	}
}
