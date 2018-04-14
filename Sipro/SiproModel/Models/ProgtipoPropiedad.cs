
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the progtipo_propiedad table.
    /// </summary>
	[Table("progtipo_propiedad")]
	public partial class Progtipopropiedad
	{
		[Key]
	    [ForeignKey("programapropiedad")]
        public virtual int programa_propiedadid { get; set; }
		[Key]
	    [ForeignKey("programatipo")]
        public virtual int programa_tipoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Programapropiedad tprogramapropiedad { get; set; }
		public virtual Programatipo tprogramatipo { get; set; }
		public virtual IEnumerable<Progtipopropiedad> progtipopropiedads { get; set; }
	}
}
