
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the HITO_RESULTADO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("HITO_RESULTADO")]
	public partial class HitoResultado
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string valorEntero { get; set; }
	    public virtual string valorString { get; set; }
	    public virtual string valorDecimal { get; set; }
	    public virtual string valorTiempo { get; set; }
	    public virtual string comentario { get; set; }
	    [ForeignKey("Hito")]
        public virtual string hitoid { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string estado { get; set; }
		public virtual Hito hitos { get; set; }
		public virtual IEnumerable<HitoResultado> hitoresultadoes { get; set; }
	}
}
