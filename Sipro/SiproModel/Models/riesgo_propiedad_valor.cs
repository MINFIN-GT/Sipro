
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
	[Table("riesgo_propiedad_valor")]
	public partial class riesgo_propiedad_valor
	{
		[Key]
	    [ForeignKey("riesgo")]
        public virtual string riesgoid { get; set; }
		[Key]
	    [ForeignKey("riesgo_propiedad")]
        public virtual string riesgo_propiedadid { get; set; }
	    public virtual string valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual string valor_decimal { get; set; }
	    public virtual string valor_tiempo { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
		public virtual riesgo_propiedad riesgo_propiedads { get; set; }
		public virtual riesgo riesgos { get; set; }
		public virtual IEnumerable<riesgo_propiedad_valor> riesgopropiedadvalors { get; set; }
	}
}
