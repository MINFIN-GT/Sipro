
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the SUBCOMPONENTE_PROPIEDAD_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("SUBCOMPONENTE_PROPIEDAD_VALOR")]
	public partial class SubcomponentePropiedadValor
	{
		[Key]
	    [ForeignKey("Subcomponente")]
        public virtual string subcomponenteid { get; set; }
		[Key]
	    [Column("SUBCOMPONENTE_PROPIEDADID")]
	    [ForeignKey("SubcomponentePropiedad")]
        public virtual string subcomponentePropiedadid { get; set; }
	    [Column("VALOR_STRING")]
	    public virtual string valorString { get; set; }
	    [Column("VALOR_ENTERO")]
	    public virtual string valorEntero { get; set; }
	    [Column("VALOR_DECIMAL")]
	    public virtual string valorDecimal { get; set; }
	    [Column("VALOR_TIEMPO")]
	    public virtual string valorTiempo { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
		public virtual SubcomponentePropiedad subcomponentePropiedads { get; set; }
		public virtual Subcomponente subcomponentes { get; set; }
		public virtual IEnumerable<SubcomponentePropiedadValor> subcomponentepropiedadvalors { get; set; }
	}
}
