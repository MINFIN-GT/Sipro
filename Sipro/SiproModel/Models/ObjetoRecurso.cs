
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the objeto_recurso table.
    /// </summary>
	[Table("objeto_recurso")]
	public partial class Objetorecurso
	{
		[Key]
	    [ForeignKey("Recurso")]
        public virtual int recursoid { get; set; }
		[Key]
	    public virtual int objetoid { get; set; }
		[Key]
	    public virtual int objeto_tipo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
		public virtual Recurso trecurso { get; set; }
		public virtual IEnumerable<Objetorecurso> objetorecursoes { get; set; }
	}
}
