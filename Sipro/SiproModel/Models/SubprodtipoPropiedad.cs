
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the subprodtipo_propiedad table.
    /// </summary>
	[Table("subprodtipo_propiedad")]
	public partial class Subprodtipopropiedad
	{
		[Key]
	    [ForeignKey("subproductotipo")]
        public virtual int subproducto_tipoid { get; set; }
		[Key]
	    [ForeignKey("subproductopropiedad")]
        public virtual int subproducto_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Subproductotipo tsubproductotipo { get; set; }
		public virtual Subproductopropiedad tsubproductopropiedad { get; set; }
		public virtual IEnumerable<Subprodtipopropiedad> subprodtipopropiedads { get; set; }
	}
}
