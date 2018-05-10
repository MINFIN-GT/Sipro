
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the COMPONENTE_USUARIO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("COMPONENTE_USUARIO")]
	public partial class ComponenteUsuario
	{
		[Key]
	    [ForeignKey("Componente")]
        public virtual Int32 componenteid { get; set; }
		[Key]
	    public virtual string usuario { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		public virtual Componente componentes { get; set; }
		public virtual IEnumerable<ComponenteUsuario> componenteusuarios { get; set; }
	}
}