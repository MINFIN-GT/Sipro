
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the proyecto_rol_colaborador table.
    /// </summary>
	[Table("proyecto_rol_colaborador")]
	public partial class Proyectorolcolaborador
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("Colaborador")]
        public virtual int colaboradorid { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual int proyectoid { get; set; }
	    [ForeignKey("Rolunidadejecutora")]
        public virtual int rol_unidad_ejecutoraid { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Colaborador tcolaborador { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual Rolunidadejecutora trolunidadejecutora { get; set; }
		public virtual IEnumerable<Proyectorolcolaborador> proyectorolcolaboradors { get; set; }
	}
}
