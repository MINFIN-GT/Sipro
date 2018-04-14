
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the prodtipo_propiedad table.
    /// </summary>
	[Table("prodtipo_propiedad")]
	public partial class Prodtipopropiedad
	{
		[Key]
	    [ForeignKey("productotipo")]
        public virtual int producto_tipoid { get; set; }
		[Key]
	    [ForeignKey("productopropiedad")]
        public virtual int producto_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Productopropiedad tproductopropiedad { get; set; }
		public virtual Productotipo tproductotipo { get; set; }
		public virtual IEnumerable<Prodtipopropiedad> prodtipopropiedads { get; set; }
	}
}
