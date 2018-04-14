
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the plan_adquisicion table.
    /// </summary>
	[Table("plan_adquisicion")]
	public partial class Planadquisicion
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("tipoadquisicion")]
        public virtual int tipo_adquisicion { get; set; }
	    [ForeignKey("categoriaadquisicion")]
        public virtual int? categoria_adquisicion { get; set; }
	    public virtual string unidad_medida { get; set; }
	    public virtual int? cantidad { get; set; }
	    public virtual decimal? total { get; set; }
	    public virtual decimal? precio_unitario { get; set; }
	    public virtual byte[] preparacion_doc_planificado { get; set; }
	    public virtual byte[] preparacion_doc_real { get; set; }
	    public virtual byte[] lanzamiento_evento_planificado { get; set; }
	    public virtual byte[] lanzamiento_evento_real { get; set; }
	    public virtual byte[] recepcion_ofertas_planificado { get; set; }
	    public virtual byte[] recepcion_ofertas_real { get; set; }
	    public virtual byte[] adjudicacion_planificado { get; set; }
	    public virtual byte[] adjudicacion_real { get; set; }
	    public virtual byte[] firma_contrato_planificado { get; set; }
	    public virtual byte[] firma_contrato_real { get; set; }
	    public virtual int objeto_id { get; set; }
	    public virtual int objeto_tipo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual int? bloqueado { get; set; }
	    public virtual string numero_contrato { get; set; }
	    public virtual decimal monto_contrato { get; set; }
	    public virtual long? nog { get; set; }
	    public virtual int? tipo_revision { get; set; }
		public virtual Categoriaadquisicion tcategoriaadquisicion { get; set; }
		public virtual Tipoadquisicion ttipoadquisicion { get; set; }
		public virtual IEnumerable<Planadquisicion> planadquisicions { get; set; }
	}
}
