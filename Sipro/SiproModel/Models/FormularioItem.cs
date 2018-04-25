
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the FORMULARIO_ITEM table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("FORMULARIO_ITEM")]
	public partial class FormularioItem
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string texto { get; set; }
	    [ForeignKey("Formulario")]
        public virtual string formularioid { get; set; }
	    public virtual string orden { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZACION")]
	    public virtual string usuarioActualizacion { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    [Column("FORMULARIO_ITEM_TIPOID")]
	    [ForeignKey("FormularioItemTipo")]
        public virtual string formularioItemTipoid { get; set; }
		public virtual Formulario formularios { get; set; }
		public virtual FormularioItemTipo formularioItemTipos { get; set; }
		public virtual IEnumerable<FormularioItem> formularioitems { get; set; }
	}
}
