
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the FORMULARIO_ITEM_TIPO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("FORMULARIO_ITEM_TIPO")]
	public partial class FormularioItemTipo
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual Int32? estado { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual byte[] fechaCreacion { get; set; }
	    [Column("USUARIO_ACTUALIZACION")]
	    public virtual string usuarioActualizacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual byte[] fechaActualizacion { get; set; }
	    [Column("DATO_TIPOID")]
	    [ForeignKey("DatoTipo")]
        public virtual Int32 datoTipoid { get; set; }
		public virtual DatoTipo datoTipos { get; set; }
		public virtual IEnumerable<FormularioItemTipo> formularioitemtipoes { get; set; }
	}
}
