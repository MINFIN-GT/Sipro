
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the informe_presupuesto table.
    /// </summary>
	[Table("informe_presupuesto")]
	public partial class Informepresupuesto
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual int id_prestamo { get; set; }
	    public virtual int? tipo_presupuesto { get; set; }
	    public virtual int objeto_tipo_id { get; set; }
	    public virtual int objeto_tipo { get; set; }
	    public virtual int? posicion_arbol { get; set; }
	    public virtual string objeto_nombre { get; set; }
	    public virtual int id_predecesor { get; set; }
	    public virtual int objeto_tipo_predecesor { get; set; }
	    public virtual decimal? mes1 { get; set; }
	    public virtual decimal? mes2 { get; set; }
	    public virtual decimal? mes3 { get; set; }
	    public virtual decimal? mes4 { get; set; }
	    public virtual decimal? mes5 { get; set; }
	    public virtual decimal? mes6 { get; set; }
	    public virtual decimal? mes7 { get; set; }
	    public virtual decimal? mes8 { get; set; }
	    public virtual decimal? mes9 { get; set; }
	    public virtual decimal? mes10 { get; set; }
	    public virtual decimal? mes11 { get; set; }
	    public virtual decimal? mes12 { get; set; }
	    public virtual decimal? total { get; set; }
	    public virtual byte[] anio { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
		public virtual IEnumerable<Informepresupuesto> informepresupuestoes { get; set; }
	}
}
