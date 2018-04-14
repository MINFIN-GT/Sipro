
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the objeto_riesgo table.
    /// </summary>
	[Table("objeto_riesgo")]
	public partial class Objetoriesgo
	{
		[Key]
	    [ForeignKey("riesgo")]
        public virtual int riesgoid { get; set; }
		[Key]
	    public virtual int objeto_id { get; set; }
		[Key]
	    public virtual int objeto_tipo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Riesgo triesgo { get; set; }
		public virtual IEnumerable<Objetoriesgo> objetoriesgoes { get; set; }
	}
}
