
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the matriz_raci table.
    /// </summary>
	[Table("matriz_raci")]
	public partial class Matrizraci
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual int proyectoid { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual IEnumerable<Matrizraci> matrizracis { get; set; }
	}
}
