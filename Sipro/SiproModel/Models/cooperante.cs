
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the cooperante table.
    /// </summary>
	[Table("cooperante")]
	public partial class Cooperante
	{
		[Key]
	    public virtual int codigo { get; set; }
	    public virtual string siglas { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		[Key]
	    public virtual int ejercicio { get; set; }
		public virtual IEnumerable<Cooperante> cooperantes { get; set; }
	}
}
