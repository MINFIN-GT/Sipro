
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the recurso table.
    /// </summary>
	[Table("recurso")]
	public partial class Recurso
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    [ForeignKey("Recursotipo")]
        public virtual int recurso_tipoid { get; set; }
	    [ForeignKey("Recursounidadmedida")]
        public virtual int recurso_unidad_medidaid { get; set; }
		public virtual Recursounidadmedida trecursounidadmedida { get; set; }
		public virtual Recursotipo trecursotipo { get; set; }
		public virtual IEnumerable<Recurso> recursoes { get; set; }
	}
}
