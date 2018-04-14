
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the documento table.
    /// </summary>
	[Table("documento")]
	public partial class Documento
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string extension { get; set; }
	    public virtual int id_tipo_objeto { get; set; }
	    public virtual int id_objeto { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual IEnumerable<Documento> documentoes { get; set; }
	}
}
