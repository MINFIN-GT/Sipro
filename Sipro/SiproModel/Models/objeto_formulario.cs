
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the OBJETO_FORMULARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("objeto_formulario")]
	public partial class objeto_formulario
	{
		[Key]
	    [ForeignKey("formulario")]
        public virtual string formularioid { get; set; }
		[Key]
	    public virtual string objeto_tipo { get; set; }
		[Key]
	    public virtual string objeto_id { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual formulario formularios { get; set; }
		public virtual IEnumerable<objeto_formulario> objetoformularios { get; set; }
	}
}
