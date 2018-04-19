
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the ROL_UNIDAD_EJECUTORA table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("ROL_UNIDAD_EJECUTORA")]
	public partial class RolUnidadEjecutora
	{
		[Key]
	    public virtual string id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string estado { get; set; }
	    public virtual string usuarioCreo { get; set; }
	    public virtual string usuarioActualizo { get; set; }
	    public virtual string fechaCreacion { get; set; }
	    public virtual string fechaActualizacion { get; set; }
	    public virtual string rolPredeterminado { get; set; }
	}
}
