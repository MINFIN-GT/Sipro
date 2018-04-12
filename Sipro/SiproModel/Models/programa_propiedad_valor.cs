namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.programa_propiedad_valor")]
    public partial class programa_propiedad_valor
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int programa_propiedadid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int programaid { get; set; }

        [StringLength(4000)]
        public string valor_string { get; set; }

        public int? valor_entero { get; set; }

        public decimal? valor_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? valor_tiempo { get; set; }

        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int? estado { get; set; }

        public virtual programa programa { get; set; }

        public virtual programa_propiedad programa_propiedad { get; set; }
    }
}
