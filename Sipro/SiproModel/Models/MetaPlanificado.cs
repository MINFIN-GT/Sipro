
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the META_PLANIFICADO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("META_PLANIFICADO")]
	public partial class MetaPlanificado
	{
		[Key]
	    [ForeignKey("Meta")]
        public virtual Int32 metaid { get; set; }
		[Key]
	    public virtual Int32 ejercicio { get; set; }
	    [Column("ENERO_ENTERO")]
	    public virtual Int32? eneroEntero { get; set; }
	    [Column("ENERO_STRING")]
	    public virtual string eneroString { get; set; }
	    [Column("ENERO_DECIMAL")]
	    public virtual decimal? eneroDecimal { get; set; }
	    [Column("ENERO_TIEMPO")]
	    public virtual byte[] eneroTiempo { get; set; }
	    [Column("FEBRERO_ENTERO")]
	    public virtual Int32? febreroEntero { get; set; }
	    [Column("FEBRERO_STRING")]
	    public virtual string febreroString { get; set; }
	    [Column("FEBRERO_DECIMAL")]
	    public virtual decimal? febreroDecimal { get; set; }
	    [Column("FEBRERO_TIEMPO")]
	    public virtual byte[] febreroTiempo { get; set; }
	    [Column("MARZO_ENTERO")]
	    public virtual Int32? marzoEntero { get; set; }
	    [Column("MARZO_STRING")]
	    public virtual string marzoString { get; set; }
	    [Column("MARZO_DECIMAL")]
	    public virtual decimal? marzoDecimal { get; set; }
	    [Column("MARZO_TIEMPO")]
	    public virtual byte[] marzoTiempo { get; set; }
	    [Column("ABRIL_ENTERO")]
	    public virtual Int32? abrilEntero { get; set; }
	    [Column("ABRIL_STRING")]
	    public virtual string abrilString { get; set; }
	    [Column("ABRIL_DECIMAL")]
	    public virtual decimal? abrilDecimal { get; set; }
	    [Column("ABRIL_TIEMPO")]
	    public virtual byte[] abrilTiempo { get; set; }
	    [Column("MAYO_ENTERO")]
	    public virtual Int32? mayoEntero { get; set; }
	    [Column("MAYO_STRING")]
	    public virtual string mayoString { get; set; }
	    [Column("MAYO_DECIMAL")]
	    public virtual decimal? mayoDecimal { get; set; }
	    [Column("MAYO_TIEMPO")]
	    public virtual byte[] mayoTiempo { get; set; }
	    [Column("JUNIO_ENTERO")]
	    public virtual Int32? junioEntero { get; set; }
	    [Column("JUNIO_STRING")]
	    public virtual string junioString { get; set; }
	    [Column("JUNIO_DECIMAL")]
	    public virtual decimal? junioDecimal { get; set; }
	    [Column("JUNIO_TIEMPO")]
	    public virtual byte[] junioTiempo { get; set; }
	    [Column("JULIO_ENTERO")]
	    public virtual Int32? julioEntero { get; set; }
	    [Column("JULIO_STRING")]
	    public virtual string julioString { get; set; }
	    [Column("JULIO_DECIMAL")]
	    public virtual decimal? julioDecimal { get; set; }
	    [Column("JULIO_TIEMPO")]
	    public virtual byte[] julioTiempo { get; set; }
	    [Column("AGOSTO_ENTERO")]
	    public virtual Int32? agostoEntero { get; set; }
	    [Column("AGOSTO_STRING")]
	    public virtual string agostoString { get; set; }
	    [Column("AGOSTO_DECIMAL")]
	    public virtual decimal? agostoDecimal { get; set; }
	    [Column("AGOSTO_TIEMPO")]
	    public virtual byte[] agostoTiempo { get; set; }
	    [Column("SEPTIEMBRE_ENTERO")]
	    public virtual Int32? septiembreEntero { get; set; }
	    [Column("SEPTIEMBRE_STRING")]
	    public virtual string septiembreString { get; set; }
	    [Column("SEPTIEMBRE_DECIMAL")]
	    public virtual decimal? septiembreDecimal { get; set; }
	    [Column("SEPTIEMBRE_TIEMPO")]
	    public virtual byte[] septiembreTiempo { get; set; }
	    [Column("OCTUBRE_ENTERO")]
	    public virtual Int32? octubreEntero { get; set; }
	    [Column("OCTUBRE_STRING")]
	    public virtual string octubreString { get; set; }
	    [Column("OCTUBRE_DECIMAL")]
	    public virtual decimal? octubreDecimal { get; set; }
	    [Column("OCTUBRE_TIEMPO")]
	    public virtual byte[] octubreTiempo { get; set; }
	    [Column("NOVIEMBRE_ENTERO")]
	    public virtual Int32? noviembreEntero { get; set; }
	    [Column("NOVIEMBRE_STRING")]
	    public virtual string noviembreString { get; set; }
	    [Column("NOVIEMBRE_DECIMAL")]
	    public virtual decimal? noviembreDecimal { get; set; }
	    [Column("NOVIEMBRE_TIEMPO")]
	    public virtual byte[] noviembreTiempo { get; set; }
	    [Column("DICIEMBRE_ENTERO")]
	    public virtual Int32? diciembreEntero { get; set; }
	    [Column("DICIEMBRE_STRING")]
	    public virtual string diciembreString { get; set; }
	    [Column("DICIEMBRE_DECIMAL")]
	    public virtual decimal? diciembreDecimal { get; set; }
	    [Column("DICIEMBRE_TIEMPO")]
	    public virtual byte[] diciembreTiempo { get; set; }
	    public virtual Int32 estado { get; set; }
	    public virtual string usuario { get; set; }
	    [Column("FECHA_INGRESO")]
	    public virtual byte[] fechaIngreso { get; set; }
		public virtual Meta metas { get; set; }
		public virtual IEnumerable<MetaPlanificado> metaplanificadoes { get; set; }
	}
}
