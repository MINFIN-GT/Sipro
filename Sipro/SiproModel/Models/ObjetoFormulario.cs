
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the objeto_formulario table.
    /// </summary>
	[Table("objeto_formulario")]
	public partial class Objetoformulario
	{
		[Key]
	    [ForeignKey("formulario")]
        public virtual int formularioid { get; set; }
		[Key]
	    public virtual int objeto_tipo { get; set; }
		[Key]
	    public virtual int objeto_id { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Formulario tformulario { get; set; }
		public virtual IEnumerable<Objetoformulario> objetoformularios { get; set; }
	}
}
