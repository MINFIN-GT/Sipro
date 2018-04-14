
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the programa table.
    /// </summary>
	[Table("programa")]
	public partial class Programa
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
	    [ForeignKey("programatipo")]
        public virtual int programa_tipoid { get; set; }
		public virtual Programatipo tprogramatipo { get; set; }
		public virtual IEnumerable<Programa> programas { get; set; }
	}
}
