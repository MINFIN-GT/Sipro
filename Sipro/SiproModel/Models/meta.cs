
namespace SiproModel.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the meta table.
    /// </summary>
	[Table("meta")]
	public partial class Meta
	{
		[Key]
	    public virtual int id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    public virtual string usuario_creo { get; set; }
	    public virtual string usuario_actualizo { get; set; }
	    public virtual byte[] fecha_creacion { get; set; }
	    public virtual byte[] fecha_actualizacion { get; set; }
	    public virtual int? estado { get; set; }
	    [ForeignKey("metaunidadmedida")]
        public virtual int meta_unidad_medidaid { get; set; }
	    [ForeignKey("datotipo")]
        public virtual int dato_tipoid { get; set; }
	    public virtual int? objeto_id { get; set; }
	    public virtual int? objeto_tipo { get; set; }
	    public virtual int? meta_final_entero { get; set; }
	    public virtual string meta_final_string { get; set; }
	    public virtual decimal? meta_final_decimal { get; set; }
	    public virtual byte[] meta_final_fecha { get; set; }
		public virtual Datotipo tdatotipo { get; set; }
		public virtual Metaunidadmedida tmetaunidadmedida { get; set; }
		public virtual IEnumerable<Meta> metas { get; set; }
	}
}
