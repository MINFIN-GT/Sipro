
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ATIPO_PROPIEDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ATIPO_PROPIEDAD")]
	public partial class AtipoPropiedad
	{
		[Key]
	    [Column("ACTIVIDAD_TIPOID")]
	    [ForeignKey("ActividadTipo")]
        public virtual Int32 actividadTipoid { get; set; }
		[Key]
	    [Column("ACTIVIDAD_PROPIEDADID")]
	    [ForeignKey("ActividadPropiedad")]
        public virtual Int32 actividadPropiedadid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		public virtual ActividadTipo actividadTipos { get; set; }
		public virtual ActividadPropiedad actividadPropiedads { get; set; }
		public virtual IEnumerable<AtipoPropiedad> atipopropiedads { get; set; }
	}
}
