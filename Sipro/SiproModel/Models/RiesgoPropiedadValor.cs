
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the RIESGO_PROPIEDAD_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("RIESGO_PROPIEDAD_VALOR")]
	public partial class RiesgoPropiedadValor
	{
		[Key]
	    [ForeignKey("Riesgo")]
        public virtual Int32 riesgoid { get; set; }
		[Key]
	    [Column("RIESGO_PROPIEDADID")]
	    [ForeignKey("RiesgoPropiedad")]
        public virtual Int32 riesgoPropiedadid { get; set; }
	    [Column("VALOR_ENTERO")]
	    public virtual Int32? valorEntero { get; set; }
	    [Column("VALOR_STRING")]
	    public virtual string valorString { get; set; }
	    [Column("VALOR_DECIMAL")]
	    public virtual decimal? valorDecimal { get; set; }
	    [Column("VALOR_TIEMPO")]
	    public virtual byte[] valorTiempo { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
		public virtual RiesgoPropiedad riesgoPropiedads { get; set; }
		public virtual Riesgo riesgos { get; set; }
		public virtual IEnumerable<RiesgoPropiedadValor> riesgopropiedadvalors { get; set; }
	}
}
