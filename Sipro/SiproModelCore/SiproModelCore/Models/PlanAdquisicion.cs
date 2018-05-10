
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PLAN_ADQUISICION table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PLAN_ADQUISICION")]
	public partial class PlanAdquisicion
	{
		[Key]
	    public virtual Int64 id { get; set; }
	    [Column("TIPO_ADQUISICION")]
	    [ForeignKey("TipoAdquisicion")]
        public virtual Int64 tipoAdquisicion { get; set; }
	    [Column("CATEGORIA_ADQUISICION")]
	    [ForeignKey("CategoriaAdquisicion")]
        public virtual Int64 categoriaAdquisicion { get; set; }
	    [Column("UNIDAD_MEDIDA")]
	    public virtual string unidadMedida { get; set; }
	    public virtual Int64? cantidad { get; set; }
	    public virtual decimal? total { get; set; }
	    [Column("PRECIO_UNITARIO")]
	    public virtual decimal? precioUnitario { get; set; }
	    [Column("PREPARACION_DOC_PLANIFICADO")]
	    public virtual DateTime? preparacionDocPlanificado { get; set; }
	    [Column("PREPARACION_DOC_REAL")]
	    public virtual DateTime? preparacionDocReal { get; set; }
	    [Column("LANZAMIENTO_EVENTO_PLANIFICADO")]
	    public virtual DateTime? lanzamientoEventoPlanificado { get; set; }
	    [Column("LANZAMIENTO_EVENTO_REAL")]
	    public virtual DateTime? lanzamientoEventoReal { get; set; }
	    [Column("RECEPCION_OFERTAS_PLANIFICADO")]
	    public virtual DateTime? recepcionOfertasPlanificado { get; set; }
	    [Column("RECEPCION_OFERTAS_REAL")]
	    public virtual DateTime? recepcionOfertasReal { get; set; }
	    [Column("ADJUDICACION_PLANIFICADO")]
	    public virtual DateTime? adjudicacionPlanificado { get; set; }
	    [Column("ADJUDICACION_REAL")]
	    public virtual DateTime? adjudicacionReal { get; set; }
	    [Column("FIRMA_CONTRATO_PLANIFICADO")]
	    public virtual DateTime? firmaContratoPlanificado { get; set; }
	    [Column("FIRMA_CONTRATO_REAL")]
	    public virtual DateTime? firmaContratoReal { get; set; }
	    [Column("OBJETO_ID")]
	    public virtual Int64 objetoId { get; set; }
	    [Column("OBJETO_TIPO")]
	    public virtual Int64 objetoTipo { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32? estado { get; set; }
	    public virtual Int32? bloqueado { get; set; }
	    [Column("NUMERO_CONTRATO")]
	    public virtual string numeroContrato { get; set; }
	    [Column("MONTO_CONTRATO")]
	    public virtual decimal montoContrato { get; set; }
	    public virtual Int32? nog { get; set; }
	    [Column("TIPO_REVISION")]
	    public virtual Int32? tipoRevision { get; set; }
		public virtual TipoAdquisicion tipoAdquisicions { get; set; }
		public virtual CategoriaAdquisicion categoriaAdquisicions { get; set; }
		public virtual IEnumerable<PlanAdquisicion> planadquisicions { get; set; }
	}
}