
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the HITO_RESULTADO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("HITO_RESULTADO")]
	public partial class HitoResultado
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    [Column("VALOR_ENTERO")]
	    public virtual Int32? valorEntero { get; set; }
	    [Column("VALOR_STRING")]
	    public virtual string valorString { get; set; }
	    [Column("VALOR_DECIMAL")]
	    public virtual decimal? valorDecimal { get; set; }
	    [Column("VALOR_TIEMPO")]
	    public virtual DateTime? valorTiempo { get; set; }
	    public virtual string comentario { get; set; }
	    [ForeignKey("Hito")]
        public virtual Int32 hitoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
		public virtual Hito hitos { get; set; }
		public virtual IEnumerable<HitoResultado> hitoresultadoes { get; set; }
	}
}
