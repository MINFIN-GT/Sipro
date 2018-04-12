namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.formulario_item_opcion")]
    public partial class formulario_item_opcion
    {
        public int id { get; set; }

        public int formulario_itemid { get; set; }

        [Required]
        [StringLength(4000)]
        public string etiqueta { get; set; }

        public int? valor_entero { get; set; }

        [StringLength(4000)]
        public string valor_string { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? valor_tiempo { get; set; }

        public decimal? valor_decimal { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        public int? usuario_actualizacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public virtual formulario_item formulario_item { get; set; }
    }
}
