namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.meta_avance")]
    public partial class meta_avance
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int metaid { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "timestamp")]
        public DateTime fecha { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario { get; set; }

        public int? valor_entero { get; set; }

        [StringLength(4000)]
        public string valor_string { get; set; }

        public decimal? valor_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? valor_tiempo { get; set; }

        public int estado { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_ingreso { get; set; }

        public virtual meta meta { get; set; }
    }
}
