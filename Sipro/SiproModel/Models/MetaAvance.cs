
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the meta_avance table.
    /// </summary>
	[Table("meta_avance")]
	public partial class Metaavance
	{
		[Key]
	    [ForeignKey("meta")]
        public virtual int metaid { get; set; }
		[Key]
	    public virtual byte[] fecha { get; set; }
	    public virtual string usuario { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual int estado { get; set; }
	    public virtual byte[] fecha_ingreso { get; set; }
		public virtual Meta tmeta { get; set; }
		public virtual IEnumerable<Metaavance> metaavances { get; set; }
	}
}
