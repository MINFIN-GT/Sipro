
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the hito_resultado table.
    /// </summary>
	[Table("hito_resultado")]
	public partial class Hitoresultado
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string comentario { get; set; }
	    [ForeignKey("Hito")]
        public virtual int hitoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
		public virtual Hito thito { get; set; }
		public virtual IEnumerable<Hitoresultado> hitoresultadoes { get; set; }
	}
}
