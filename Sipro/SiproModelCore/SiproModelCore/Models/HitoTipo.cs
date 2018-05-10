
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the HITO_TIPO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("HITO_TIPO")]
	public partial class HitoTipo
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("DATO_TIPOID")]
	    [ForeignKey("DatoTipo")]
        public virtual Int32 datoTipoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZACION")]
	    public virtual string usuarioActualizacion { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		public virtual DatoTipo datoTipos { get; set; }
		public virtual IEnumerable<HitoTipo> hitotipoes { get; set; }
	}
}
