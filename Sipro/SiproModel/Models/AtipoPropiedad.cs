
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the atipo_propiedad table.
    /// </summary>
	[Table("atipo_propiedad")]
	public partial class Atipopropiedad
	{
		[Key]
	    [ForeignKey("Actividadtipo")]
        public virtual int actividad_tipoid { get; set; }
		[Key]
	    [ForeignKey("Actividadpropiedad")]
        public virtual int actividad_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Actividadpropiedad tactividadpropiedad { get; set; }
		public virtual Actividadtipo tactividadtipo { get; set; }
		public virtual IEnumerable<Atipopropiedad> atipopropiedads { get; set; }
	}
}
