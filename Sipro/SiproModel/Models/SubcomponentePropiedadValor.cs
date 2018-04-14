
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the subcomponente_propiedad_valor table.
    /// </summary>
	[Table("subcomponente_propiedad_valor")]
	public partial class Subcomponentepropiedadvalor
	{
		[Key]
	    [ForeignKey("subcomponente")]
        public virtual int subcomponenteid { get; set; }
		[Key]
	    [ForeignKey("subcomponentepropiedad")]
        public virtual int subcomponente_propiedadid { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Subcomponentepropiedad tsubcomponentepropiedad { get; set; }
		public virtual Subcomponente tsubcomponente { get; set; }
		public virtual IEnumerable<Subcomponentepropiedadvalor> subcomponentepropiedadvalors { get; set; }
	}
}
