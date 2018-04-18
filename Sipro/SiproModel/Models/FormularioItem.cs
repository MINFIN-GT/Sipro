
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the formulario_item table.
    /// </summary>
	[Table("formulario_item")]
	public partial class Formularioitem
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string texto { get; set; }
	    [ForeignKey("Formulario")]
        public virtual int formularioid { get; set; }
	    public virtual int orden { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual int? usuario_actualizacion { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    [ForeignKey("Formularioitemtipo")]
        public virtual int formulario_item_tipoid { get; set; }
		public virtual Formularioitemtipo tformularioitemtipo { get; set; }
		public virtual Formulario tformulario { get; set; }
		public virtual IEnumerable<Formularioitem> formularioitems { get; set; }
	}
}
