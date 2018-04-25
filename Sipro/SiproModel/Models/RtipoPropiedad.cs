
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the RTIPO_PROPIEDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("RTIPO_PROPIEDAD")]
	public partial class RtipoPropiedad
	{
		[Key]
	    [Column("RIESGO_TIPOID")]
	    [ForeignKey("RiesgoTipo")]
        public virtual string riesgoTipoid { get; set; }
		[Key]
	    [Column("RIESGO_PROPIEDADID")]
	    [ForeignKey("RiesgoPropiedad")]
        public virtual string riesgoPropiedadid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual RiesgoPropiedad riesgoPropiedads { get; set; }
		public virtual RiesgoTipo riesgoTipos { get; set; }
		public virtual IEnumerable<RtipoPropiedad> rtipopropiedads { get; set; }
	}
}
