
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the HITO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("HITO")]
	public partial class Hito
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual string fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string fecha { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual string proyectoid { get; set; }
	    [Column("HITO_TIPOID")]
	    [ForeignKey("HitoTipo")]
        public virtual string hitoTipoid { get; set; }
		public virtual HitoTipo hitoTipos { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual IEnumerable<Hito> hitoes { get; set; }
	}
}
