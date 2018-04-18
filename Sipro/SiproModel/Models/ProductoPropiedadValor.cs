
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the producto_propiedad_valor table.
    /// </summary>
	[Table("producto_propiedad_valor")]
	public partial class Productopropiedadvalor
	{
		[Key]
	    [ForeignKey("Productopropiedad")]
        public virtual int producto_propiedadid { get; set; }
		[Key]
	    [ForeignKey("Producto")]
        public virtual int productoid { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Producto tproducto { get; set; }
		public virtual Productopropiedad tproductopropiedad { get; set; }
		public virtual IEnumerable<Productopropiedadvalor> productopropiedadvalors { get; set; }
	}
}
