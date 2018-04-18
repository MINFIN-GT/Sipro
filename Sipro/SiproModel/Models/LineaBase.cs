
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the linea_base table.
    /// </summary>
	[Table("linea_base")]
	public partial class Lineabase
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual int proyectoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int tipo_linea_base { get; set; }
	    public virtual int? sobreescribir { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual IEnumerable<Lineabase> lineabases { get; set; }
	}
}
