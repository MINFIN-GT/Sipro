
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the formulario_item_opcion table.
    /// </summary>
	[Table("formulario_item_opcion")]
	public partial class Formularioitemopcion
	{
		[Key]
	    public virtual int id { get; set; }
	    [ForeignKey("formularioitem")]
        public virtual int formulario_itemid { get; set; }
	    public virtual string etiqueta { get; set; }
	    public virtual int? valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual int estado { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual int? usuario_actualizacion { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		public virtual Formularioitem tformularioitem { get; set; }
		public virtual IEnumerable<Formularioitemopcion> formularioitemopcions { get; set; }
	}
}
