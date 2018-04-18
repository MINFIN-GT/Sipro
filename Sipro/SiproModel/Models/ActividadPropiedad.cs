
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the actividad_propiedad table.
    /// </summary>
	[Table("actividad_propiedad")]
	public partial class Actividadpropiedad
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
	    [ForeignKey("Datotipo")]
        public virtual int dato_tipoid { get; set; }
		public virtual Datotipo tdatotipo { get; set; }
		public virtual IEnumerable<Actividadpropiedad> actividadpropiedads { get; set; }
	}
}
