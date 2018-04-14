
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the pago_planificado table.
    /// </summary>
	[Table("pago_planificado")]
	public partial class Pagoplanificado
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual byte[] fecha_pago { get; set; }
	    public virtual decimal pago { get; set; }
	    public virtual int objeto_id { get; set; }
	    public virtual int objeto_tipo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual IEnumerable<Pagoplanificado> pagoplanificadoes { get; set; }
	}
}
