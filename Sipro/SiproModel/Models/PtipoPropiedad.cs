
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ptipo_propiedad table.
    /// </summary>
	[Table("ptipo_propiedad")]
	public partial class Ptipopropiedad
	{
		[Key]
	    [ForeignKey("Proyectotipo")]
        public virtual int proyecto_tipoid { get; set; }
		[Key]
	    [ForeignKey("Proyectopropiedad")]
        public virtual int proyecto_propiedadid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Proyectopropiedad tproyectopropiedad { get; set; }
		public virtual Proyectotipo tproyectotipo { get; set; }
		public virtual IEnumerable<Ptipopropiedad> ptipopropiedads { get; set; }
	}
}
