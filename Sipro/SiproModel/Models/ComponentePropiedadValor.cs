
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the componente_propiedad_valor table.
    /// </summary>
	[Table("componente_propiedad_valor")]
	public partial class Componentepropiedadvalor
	{
		[Key]
	    [ForeignKey("componente")]
        public virtual int componenteid { get; set; }
		[Key]
	    [ForeignKey("componentepropiedad")]
        public virtual int componente_propiedadid { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Componentepropiedad tcomponentepropiedad { get; set; }
		public virtual Componente tcomponente { get; set; }
		public virtual IEnumerable<Componentepropiedadvalor> componentepropiedadvalors { get; set; }
	}
}
