
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the COMPONENTE_SIGADE table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("componente_sigade")]
	public partial class componente_sigade
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string codigo_presupuestario { get; set; }
	    public virtual string numero_componente { get; set; }
	    public virtual string monto_componente { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual string fecha_creacion { get; set; }
	    public virtual string fecha_actualizacion { get; set; }
	    public virtual string estado { get; set; }
	}
}
