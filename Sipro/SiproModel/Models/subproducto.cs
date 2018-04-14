
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the subproducto table.
    /// </summary>
	[Table("subproducto")]
	public partial class Subproducto
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int estado { get; set; }
	    public virtual long? snip { get; set; }
	    public virtual int? programa { get; set; }
	    public virtual int? subprograma { get; set; }
	    public virtual int? proyecto { get; set; }
	    public virtual int? actividad { get; set; }
	    public virtual int? obra { get; set; }
	    [ForeignKey("producto")]
        public virtual int productoid { get; set; }
	    [ForeignKey("subproductotipo")]
        public virtual int subproducto_tipoid { get; set; }
	    [ForeignKey("unidadejecutora")]
        public virtual int? unidad_ejecutoraunidad_ejecutora { get; set; }
	    public virtual string latitud { get; set; }
	    public virtual string longitud { get; set; }
	    public virtual decimal? costo { get; set; }
	    [ForeignKey("acumulacioncosto")]
        public virtual int? acumulacion_costoid { get; set; }
	    public virtual int? renglon { get; set; }
	    public virtual int? ubicacion_geografica { get; set; }
	    public virtual byte[] fecha_inicio { get; set; }
	    public virtual byte[] fecha_fin { get; set; }
	    public virtual int duracion { get; set; }
	    public virtual string duracion_dimension { get; set; }
	    public virtual int? orden { get; set; }
	    public virtual string treePath { get; set; }
	    public virtual int? nivel { get; set; }
	    [ForeignKey("unidadejecutora")]
        public virtual int? entidad { get; set; }
	    [ForeignKey("unidadejecutora")]
        public virtual int? ejercicio { get; set; }
	    public virtual byte[] fecha_inicio_real { get; set; }
	    public virtual byte[] fecha_fin_real { get; set; }
	    public virtual int inversion_nueva { get; set; }
		public virtual Producto tproducto { get; set; }
		public virtual Subproductotipo tsubproductotipo { get; set; }
		public virtual Unidadejecutora tunidadejecutora { get; set; }
		public virtual Acumulacioncosto tacumulacioncosto { get; set; }
		public virtual IEnumerable<Subproducto> subproductoes { get; set; }
	}
}
