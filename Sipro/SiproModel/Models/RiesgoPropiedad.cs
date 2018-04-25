
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the RIESGO_PROPIEDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("RIESGO_PROPIEDAD")]
	public partial class RiesgoPropiedad
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    [Column("DATO_TIPOID")]
	    [ForeignKey("DatoTipo")]
        public virtual string datoTipoid { get; set; }
		public virtual DatoTipo datoTipos { get; set; }
		public virtual IEnumerable<RiesgoPropiedad> riesgopropiedads { get; set; }
	}
}
