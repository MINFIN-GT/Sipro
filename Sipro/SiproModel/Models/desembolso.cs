
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the desembolso table.
    /// </summary>
	[Table("desembolso")]
	public partial class Desembolso
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual byte[] fecha { get; set; }
	    public virtual int estado { get; set; }
	    public virtual decimal monto { get; set; }
	    public virtual decimal tipo_cambio { get; set; }
	    public virtual int? monto_moneda_origen { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    [ForeignKey("proyecto")]
        public virtual int proyectoid { get; set; }
	    [ForeignKey("desembolsotipo")]
        public virtual int desembolso_tipoid { get; set; }
	    [ForeignKey("tipomoneda")]
        public virtual int tipo_monedaid { get; set; }
		public virtual Proyecto tproyecto { get; set; }
		public virtual Desembolsotipo tdesembolsotipo { get; set; }
		public virtual Tipomoneda ttipomoneda { get; set; }
		public virtual IEnumerable<Desembolso> desembolsoes { get; set; }
	}
}
