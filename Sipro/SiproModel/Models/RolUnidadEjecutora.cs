
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the rol_unidad_ejecutora table.
    /// </summary>
	[Table("rol_unidad_ejecutora")]
	public partial class Rolunidadejecutora
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? rol_predeterminado { get; set; }
		public virtual IEnumerable<Rolunidadejecutora> rolunidadejecutoras { get; set; }
	}
}
