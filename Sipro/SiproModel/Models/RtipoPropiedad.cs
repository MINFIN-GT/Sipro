
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the rtipo_propiedad table.
    /// </summary>
	[Table("rtipo_propiedad")]
	public partial class Rtipopropiedad
	{
		[Key]
	    [ForeignKey("Riesgotipo")]
        public virtual int riesgo_tipoid { get; set; }
		[Key]
	    [ForeignKey("Riesgopropiedad")]
        public virtual int riesgo_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Riesgotipo triesgotipo { get; set; }
		public virtual Riesgopropiedad triesgopropiedad { get; set; }
		public virtual IEnumerable<Rtipopropiedad> rtipopropiedads { get; set; }
	}
}
