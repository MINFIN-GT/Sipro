
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the formulario table.
    /// </summary>
	[Table("formulario")]
	public partial class Formulario
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string codigo { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    [ForeignKey("Formulariotipo")]
        public virtual int formulario_tipoid { get; set; }
		public virtual Formulariotipo tformulariotipo { get; set; }
		public virtual IEnumerable<Formulario> formularios { get; set; }
	}
}
