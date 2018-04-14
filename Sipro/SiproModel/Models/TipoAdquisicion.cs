
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the tipo_adquisicion table.
    /// </summary>
	[Table("tipo_adquisicion")]
	public partial class Tipoadquisicion
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual int cooperantecodigo { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual int? convenio_cdirecta { get; set; }
		public virtual IEnumerable<Tipoadquisicion> tipoadquisicions { get; set; }
	}
}
