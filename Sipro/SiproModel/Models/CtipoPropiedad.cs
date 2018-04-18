
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ctipo_propiedad table.
    /// </summary>
	[Table("ctipo_propiedad")]
	public partial class Ctipopropiedad
	{
		[Key]
	    [ForeignKey("Componentetipo")]
        public virtual int componente_tipoid { get; set; }
		[Key]
	    [ForeignKey("Componentepropiedad")]
        public virtual int componente_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Componentetipo tcomponentetipo { get; set; }
		public virtual Componentepropiedad tcomponentepropiedad { get; set; }
		public virtual IEnumerable<Ctipopropiedad> ctipopropiedads { get; set; }
	}
}
