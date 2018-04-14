
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the programa_propiedad_valor table.
    /// </summary>
	[Table("programa_propiedad_valor")]
	public partial class Programapropiedadvalor
	{
		[Key]
	    [ForeignKey("programapropiedad")]
        public virtual int programa_propiedadid { get; set; }
		[Key]
	    [ForeignKey("programa")]
        public virtual int programaid { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual Programapropiedad tprogramapropiedad { get; set; }
		public virtual Programa tprograma { get; set; }
		public virtual IEnumerable<Programapropiedadvalor> programapropiedadvalors { get; set; }
	}
}
