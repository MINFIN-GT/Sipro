
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the plan_adquisicion_pago table.
    /// </summary>
	[Table("plan_adquisicion_pago")]
	public partial class Planadquisicionpago
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("planadquisicion")]
        public virtual int plan_adquisicionid { get; set; }
	    public virtual byte[] fecha_pago { get; set; }
	    public virtual decimal? pago { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Planadquisicion tplanadquisicion { get; set; }
		public virtual IEnumerable<Planadquisicionpago> planadquisicionpagoes { get; set; }
	}
}
