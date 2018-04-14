
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the componente_sigade table.
    /// </summary>
	[Table("componente_sigade")]
	public partial class Componentesigade
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string codigo_presupuestario { get; set; }
	    public virtual int numero_componente { get; set; }
	    public virtual decimal monto_componente { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuaraio_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual IEnumerable<Componentesigade> componentesigades { get; set; }
	}
}
