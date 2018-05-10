
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the PAGO_PLANIFICADO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("PAGO_PLANIFICADO")]
	public partial class PagoPlanificado
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    [Column("FECHA_PAGO")]
	    public virtual DateTime fechaPago { get; set; }
	    public virtual decimal pago { get; set; }
	    [Column("OBJETO_ID")]
	    public virtual Int32 objetoId { get; set; }
	    [Column("OBJETO_TIPO")]
	    public virtual Int32 objetoTipo { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
	}
}