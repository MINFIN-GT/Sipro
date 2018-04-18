
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the proyecto_impacto table.
    /// </summary>
	[Table("proyecto_impacto")]
	public partial class Proyectoimpacto
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual int proyectoid { get; set; }
	    [ForeignKey("Entidad")]
        public virtual int entidadentidad { get; set; }
	    public virtual string impacto { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    [ForeignKey("Entidad")]
        public virtual int ejercicio { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual Entidad tentidad { get; set; }
		public virtual IEnumerable<Proyectoimpacto> proyectoimpactoes { get; set; }
	}
}
