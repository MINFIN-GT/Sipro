
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the riesgo table.
    /// </summary>
	[Table("riesgo")]
	public partial class Riesgo
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [ForeignKey("riesgotipo")]
        public virtual int riesgo_tipoid { get; set; }
	    public virtual decimal impacto { get; set; }
	    public virtual decimal probabilidad { get; set; }
	    public virtual decimal? impacto_monto { get; set; }
	    public virtual decimal? impacto_tiempo { get; set; }
	    public virtual string gatillo { get; set; }
	    public virtual string consecuencia { get; set; }
	    public virtual string solucion { get; set; }
	    public virtual string riesgos_segundarios { get; set; }
	    public virtual int ejecutado { get; set; }
	    public virtual byte[] fecha_ejecucion { get; set; }
	    public virtual string resultado { get; set; }
	    public virtual string observaciones { get; set; }
	    [ForeignKey("colaborador")]
        public virtual int? colaboradorid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Colaborador tcolaborador { get; set; }
		public virtual Riesgotipo triesgotipo { get; set; }
		public virtual IEnumerable<Riesgo> riesgoes { get; set; }
	}
}
