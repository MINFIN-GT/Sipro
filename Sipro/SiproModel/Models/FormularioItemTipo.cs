
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the formulario_item_tipo table.
    /// </summary>
	[Table("formulario_item_tipo")]
	public partial class Formularioitemtipo
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual int? estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual string usuario_actualizacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    [ForeignKey("datotipo")]
        public virtual int dato_tipoid { get; set; }
		public virtual Datotipo tdatotipo { get; set; }
		public virtual IEnumerable<Formularioitemtipo> formularioitemtipoes { get; set; }
	}
}
