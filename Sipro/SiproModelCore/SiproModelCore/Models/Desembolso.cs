
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the DESEMBOLSO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("DESEMBOLSO")]
	public partial class Desembolso
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual DateTime fecha { get; set; }
	    public virtual Int32 estado { get; set; }
	    public virtual decimal monto { get; set; }
	    [Column("TIPO_CAMBIO")]
	    public virtual decimal tipoCambio { get; set; }
	    [Column("MONTO_MONEDA_ORIGEN")]
	    public virtual Int64? montoMonedaOrigen { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    [ForeignKey("Proyecto")]
        public virtual Int32 proyectoid { get; set; }
	    [Column("DESEMBOLSO_TIPOID")]
	    [ForeignKey("DesembolsoTipo")]
        public virtual Int32 desembolsoTipoid { get; set; }
	    [Column("TIPO_MONEDAID")]
	    [ForeignKey("TipoMoneda")]
        public virtual Int32 tipoMonedaid { get; set; }
		public virtual Proyecto proyectos { get; set; }
		public virtual DesembolsoTipo desembolsoTipos { get; set; }
		public virtual TipoMoneda tipoMonedas { get; set; }
		public virtual IEnumerable<Desembolso> desembolsoes { get; set; }
	}
}
