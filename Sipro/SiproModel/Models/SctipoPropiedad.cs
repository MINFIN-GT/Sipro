
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the sctipo_propiedad table.
    /// </summary>
	[Table("sctipo_propiedad")]
	public partial class Sctipopropiedad
	{
		[Key]
	    [ForeignKey("subcomponentetipo")]
        public virtual int subcomponente_tipoid { get; set; }
		[Key]
	    [ForeignKey("subcomponentepropiedad")]
        public virtual int subcomponente_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Subcomponentetipo tsubcomponentetipo { get; set; }
		public virtual Subcomponentepropiedad tsubcomponentepropiedad { get; set; }
		public virtual IEnumerable<Sctipopropiedad> sctipopropiedads { get; set; }
	}
}
