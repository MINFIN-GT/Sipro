
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the actividad table.
    /// </summary>
	[Table("actividad")]
	public partial class Actividad
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual byte[] fecha_inicio { get; set; }
	    public virtual byte[] fecha_fin { get; set; }
	    public virtual int porcentaje_avance { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    [ForeignKey("Actividadtipo")]
        public virtual int actividad_tipoid { get; set; }
	    public virtual long? snip { get; set; }
	    public virtual int? programa { get; set; }
	    public virtual int? subprograma { get; set; }
	    public virtual int? proyecto { get; set; }
	    public virtual int? _actividad { get; set; }
	    public virtual int? obra { get; set; }
	    public virtual int objeto_id { get; set; }
	    public virtual int objeto_tipo { get; set; }
	    public virtual int duracion { get; set; }
	    public virtual string duracion_dimension { get; set; }
	    public virtual int? pred_objeto_id { get; set; }
	    public virtual int? pred_objeto_tipo { get; set; }
	    public virtual string latitud { get; set; }
	    public virtual string longitud { get; set; }
	    public virtual decimal? costo { get; set; }
	    [ForeignKey("Acumulacioncosto")]
        public virtual int? acumulacion_costo { get; set; }
	    public virtual int? renglon { get; set; }
	    public virtual int? ubicacion_geografica { get; set; }
	    public virtual int? orden { get; set; }
	    public virtual string treePath { get; set; }
	    public virtual int? nivel { get; set; }
	    public virtual int? proyecto_base { get; set; }
	    public virtual int? componente_base { get; set; }
	    public virtual int? producto_base { get; set; }
	    public virtual byte[] fecha_inicio_real { get; set; }
	    public virtual byte[] fecha_fin_real { get; set; }
	    public virtual int inversion_nueva { get; set; }
		public virtual Actividadtipo tactividadtipo { get; set; }
		public virtual Acumulacioncosto tacumulacioncosto { get; set; }
		public virtual IEnumerable<Actividad> actividads { get; set; }
	}
}
