
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the meta_planificado table.
    /// </summary>
	[Table("meta_planificado")]
	public partial class Metaplanificado
	{
		[Key]
	    [ForeignKey("meta")]
        public virtual int metaid { get; set; }
		[Key]
	    public virtual int ejercicio { get; set; }
	    public virtual int? enero_entero { get; set; }
	    public virtual string enero_string { get; set; }
	    public virtual decimal? enero_decimal { get; set; }
	    public virtual byte[] enero_tiempo { get; set; }
	    public virtual int? febrero_entero { get; set; }
	    public virtual string febrero_string { get; set; }
	    public virtual decimal? febrero_decimal { get; set; }
	    public virtual byte[] febrero_tiempo { get; set; }
	    public virtual int? marzo_entero { get; set; }
	    public virtual string marzo_string { get; set; }
	    public virtual decimal? marzo_decimal { get; set; }
	    public virtual byte[] marzo_tiempo { get; set; }
	    public virtual int? abril_entero { get; set; }
	    public virtual string abril_string { get; set; }
	    public virtual decimal? abril_decimal { get; set; }
	    public virtual byte[] abril_tiempo { get; set; }
	    public virtual int? mayo_entero { get; set; }
	    public virtual string mayo_string { get; set; }
	    public virtual decimal? mayo_decimal { get; set; }
	    public virtual byte[] mayo_tiempo { get; set; }
	    public virtual int? junio_entero { get; set; }
	    public virtual string junio_string { get; set; }
	    public virtual decimal? junio_decimal { get; set; }
	    public virtual byte[] junio_tiempo { get; set; }
	    public virtual int? julio_entero { get; set; }
	    public virtual string julio_string { get; set; }
	    public virtual decimal? julio_decimal { get; set; }
	    public virtual byte[] julio_tiempo { get; set; }
	    public virtual int? agosto_entero { get; set; }
	    public virtual string agosto_string { get; set; }
	    public virtual decimal? agosto_decimal { get; set; }
	    public virtual byte[] agosto_tiempo { get; set; }
	    public virtual int? septiembre_entero { get; set; }
	    public virtual string septiembre_string { get; set; }
	    public virtual decimal? septiembre_decimal { get; set; }
	    public virtual byte[] septiembre_tiempo { get; set; }
	    public virtual int? octubre_entero { get; set; }
	    public virtual string octubre_string { get; set; }
	    public virtual decimal? octubre_decimal { get; set; }
	    public virtual byte[] octubre_tiempo { get; set; }
	    public virtual int? noviembre_entero { get; set; }
	    public virtual string noviembre_string { get; set; }
	    public virtual decimal? noviembre_decimal { get; set; }
	    public virtual byte[] noviembre_tiempo { get; set; }
	    public virtual int? diciembre_entero { get; set; }
	    public virtual string diciembre_string { get; set; }
	    public virtual decimal? diciembre_decimal { get; set; }
	    public virtual byte[] diciembre_tiempo { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario { get; set; }
	    public virtual byte[] fecha_ingreso { get; set; }
		public virtual Meta tmeta { get; set; }
		public virtual IEnumerable<Metaplanificado> metaplanificadoes { get; set; }
	}
}
