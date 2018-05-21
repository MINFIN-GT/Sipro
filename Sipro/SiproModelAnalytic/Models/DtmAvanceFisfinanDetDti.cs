
namespace SiproModelAnalytic.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the DTM_AVANCE_FISFINAN_DET_DTI table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("DTM_AVANCE_FISFINAN_DET_DTI")]
	public partial class DtmAvanceFisfinanDetDti
	{
	    [Column("EJERCICIO_FISCAL")]
	    public virtual Int64 ejercicioFiscal { get; set; }
	    [Column("MES_DESEMBOLSO")]
	    public virtual string mesDesembolso { get; set; }
	    [Column("CODIGO_PRESUPUESTARIO")]
	    public virtual string codigoPresupuestario { get; set; }
	    [Column("ENTIDAD_SICOIN")]
	    public virtual Int32? entidadSicoin { get; set; }
	    [Column("UNIDAD_EJECUTORA_SICOIN")]
	    public virtual Int32? unidadEjecutoraSicoin { get; set; }
	    [Column("MONEDA_DESEMBOLSO")]
	    public virtual string monedaDesembolso { get; set; }
	    [Column("DESEMBOLSOS_MES_MONEDA")]
	    public virtual decimal? desembolsosMesMoneda { get; set; }
	    [Column("TC_MON_USD")]
	    public virtual decimal? tcMonUsd { get; set; }
	    [Column("DESEMBOLSOS_MES_USD")]
	    public virtual decimal? desembolsosMesUsd { get; set; }
	    [Column("TC_USD_GTQ")]
	    public virtual decimal? tcUsdGtq { get; set; }
	    [Column("DESEMBOLSOS_MES_GTQ")]
	    public virtual decimal? desembolsosMesGtq { get; set; }
	}
}