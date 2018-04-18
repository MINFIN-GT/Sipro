
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the formulario_item_valor table.
    /// </summary>
	[Table("formulario_item_valor")]
	public partial class Formularioitemvalor
	{
		[Key]
	    [ForeignKey("Formularioitem")]
        public virtual int formulario_itemid { get; set; }
		[Key]
	    [ForeignKey("Objetoformulario")]
        public virtual int objeto_formularioformularioid { get; set; }
		[Key]
	    [ForeignKey("Objetoformulario")]
        public virtual int objeto_formularioobjeto_tipoid { get; set; }
	    public virtual int valor_entero { get; set; }
	    public virtual string valor_string { get; set; }
	    public virtual byte[] valor_tiempo { get; set; }
	    public virtual decimal? valor_decimal { get; set; }
	    public virtual int proyectoid { get; set; }
	    public virtual int componenteid { get; set; }
	    public virtual int productoid { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
		[Key]
	    [ForeignKey("Objetoformulario")]
        public virtual int objeto_formularioobjeto_id { get; set; }
		public virtual Formularioitem tformularioitem { get; set; }
		public virtual Objetoformulario tobjetoformulario { get; set; }
		public virtual IEnumerable<Formularioitemvalor> formularioitemvalors { get; set; }
	}
}
