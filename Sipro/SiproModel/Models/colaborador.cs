
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the COLABORADOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("COLABORADOR")]
	public partial class Colaborador
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string pnombre { get; set; }
	    public virtual string snombre { get; set; }
	    public virtual string papellido { get; set; }
	    public virtual string sapellido { get; set; }
	    public virtual string cui { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string ueunidadEjecutora { get; set; }
	    [ForeignKey("Usuario")]
        public virtual string usuariousuario { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string ejercicio { get; set; }
	    [ForeignKey("UnidadEjecutora")]
        public virtual string entidad { get; set; }
		public virtual Usuario usuarios { get; set; }
		public virtual UnidadEjecutora unidadEjecutoras { get; set; }
		public virtual IEnumerable<Colaborador> colaboradors { get; set; }
	}
}
