
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
	    public virtual string id { get; set; }
	    [ForeignKey("TipoAdquisicion")]
        public virtual string tipoAdquisicion { get; set; }
	    [ForeignKey("CategoriaAdquisicion")]
        public virtual string categoriaAdquisicion { get; set; }
	    public virtual string unidadMedida { get; set; }
	    public virtual string cantidad { get; set; }
	    public virtual string total { get; set; }
	    public virtual string precioUnitario { get; set; }
	    public virtual string preparacionDocPlanificado { get; set; }
	    public virtual string preparacionDocReal { get; set; }
	    public virtual string lanzamientoEventoPlanificado { get; set; }
	    public virtual string lanzamientoEventoReal { get; set; }
	    public virtual string recepcionOfertasPlanificado { get; set; }
	    public virtual string recepcionOfertasReal { get; set; }
	    public virtual string adjudicacionPlanificado { get; set; }
	    public virtual string adjudicacionReal { get; set; }
	    public virtual string firmaContratoPlanificado { get; set; }
	    public virtual string firmaContratoReal { get; set; }
	    public virtual string objetoId { get; set; }
	    public virtual string objetoTipo { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string bloqueado { get; set; }
	    public virtual string numeroContrato { get; set; }
	    public virtual string montoContrato { get; set; }
	    public virtual string nog { get; set; }
	    public virtual string tipoRevision { get; set; }
		public virtual CategoriaAdquisicion categoriaAdquisicions { get; set; }
		public virtual TipoAdquisicion tipoAdquisicions { get; set; }
		public virtual IEnumerable<PlanAdquisicion> planadquisicions { get; set; }
	}
}
