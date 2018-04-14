
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the hito table.
    /// </summary>
	[Table("hito")]
	public partial class Hito
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual byte[] fecha { get; set; }
	    [ForeignKey("proyecto")]
        public virtual int proyectoid { get; set; }
	    [ForeignKey("hitotipo")]
        public virtual int hito_tipoid { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual Hitotipo thitotipo { get; set; }
		public virtual IEnumerable<Hito> hitoes { get; set; }
	}
}
