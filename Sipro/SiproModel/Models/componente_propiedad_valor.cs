
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the COMPONENTE_PROPIEDAD_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("componente_propiedad_valor")]
	public partial class componente_propiedad_valor
	{
		[Key]
	    [ForeignKey("componente")]
        public virtual string componenteid { get; set; }
		[Key]
	    [ForeignKey("componente_propiedad")]
        public virtual string componente_propiedadid { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual string valor_entero { get; set; }
	    public virtual string valor_decimal { get; set; }
	    public virtual string valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
		public virtual componente_propiedad componente_propiedads { get; set; }
		public virtual componente componentes { get; set; }
		public virtual IEnumerable<componente_propiedad_valor> componentepropiedadvalors { get; set; }
	}
}
