
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the unidad_ejecutora table.
    /// </summary>
	[Table("unidad_ejecutora")]
	public partial class Unidadejecutora
	{
		[Key]
	    public virtual int unidad_ejecutora { get; set; }
	    public virtual string nombre { get; set; }
		[Key]
	    [ForeignKey("Entidad")]
        public virtual int entidadentidad { get; set; }
		[Key]
	    [ForeignKey("Entidad")]
        public virtual int ejercicio { get; set; }
		public virtual Entidad tentidad { get; set; }
		public virtual IEnumerable<Unidadejecutora> unidadejecutoras { get; set; }
	}
}
