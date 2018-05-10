
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ACTIVIDAD_PROPIEDAD_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ACTIVIDAD_PROPIEDAD_VALOR")]
	public partial class ActividadPropiedadValor
	{
		[Key]
	    [ForeignKey("Actividad")]
        public virtual Int32 actividadid { get; set; }
		[Key]
	    [Column("ACTIVIDAD_PROPIEDADID")]
	    [ForeignKey("ActividadPropiedad")]
        public virtual Int32 actividadPropiedadid { get; set; }
	    [Column("VALOR_ENTERO")]
	    public virtual Int32? valorEntero { get; set; }
	    [Column("VALOR_STRING")]
	    public virtual string valorString { get; set; }
	    [Column("VALOR_DECIMAL")]
	    public virtual decimal? valorDecimal { get; set; }
	    [Column("VALOR_TIEMPO")]
	    public virtual DateTime? valorTiempo { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime? fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32? estado { get; set; }
		public virtual Actividad actividads { get; set; }
		public virtual ActividadPropiedad actividadPropiedads { get; set; }
		public virtual IEnumerable<ActividadPropiedadValor> actividadpropiedadvalors { get; set; }
	}
}
