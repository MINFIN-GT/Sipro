
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the META_AVANCE table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("META_AVANCE")]
	public partial class MetaAvance
	{
	    [ForeignKey("Meta")]
        public virtual Int32 metaid { get; set; }
	    public virtual DateTime fecha { get; set; }
	    public virtual string usuario { get; set; }
	    [Column("VALOR_ENTERO")]
	    public virtual Int32? valorEntero { get; set; }
	    [Column("VALOR_STRING")]
	    public virtual string valorString { get; set; }
	    [Column("VALOR_DECIMAL")]
	    public virtual decimal? valorDecimal { get; set; }
	    [Column("VALOR_TIEMPO")]
	    public virtual DateTime? valorTiempo { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("FECHA_INGRESO")]
	    public virtual DateTime fechaIngreso { get; set; }
		public virtual Meta metas { get; set; }
		public virtual IEnumerable<MetaAvance> metaavances { get; set; }
	}
}