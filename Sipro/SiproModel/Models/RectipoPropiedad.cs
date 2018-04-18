
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the rectipo_propiedad table.
    /// </summary>
	[Table("rectipo_propiedad")]
	public partial class Rectipopropiedad
	{
		[Key]
	    [ForeignKey("Recursopropiedad")]
        public virtual int recurso_propiedadid { get; set; }
		[Key]
	    [ForeignKey("Recursotipo")]
        public virtual int recurso_tipoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Recursotipo trecursotipo { get; set; }
		public virtual Recursopropiedad trecursopropiedad { get; set; }
		public virtual IEnumerable<Rectipopropiedad> rectipopropiedads { get; set; }
	}
}
