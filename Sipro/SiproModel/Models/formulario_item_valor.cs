namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.formulario_item_valor")]
    public partial class formulario_item_valor
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int formulario_itemid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int objeto_formularioformularioid { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int objeto_formularioobjeto_tipoid { get; set; }

        public int valor_entero { get; set; }

        [StringLength(4000)]
        public string valor_string { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? valor_tiempo { get; set; }

        public decimal? valor_decimal { get; set; }

        public int proyectoid { get; set; }

        public int componenteid { get; set; }

        public int productoid { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int objeto_formularioobjeto_id { get; set; }

        public virtual formulario_item formulario_item { get; set; }

        public virtual objeto_formulario objeto_formulario { get; set; }
    }
}
