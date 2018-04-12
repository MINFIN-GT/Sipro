namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.subcomponente_propiedad_valor")]
    public partial class subcomponente_propiedad_valor
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int subcomponenteid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int subcomponente_propiedadid { get; set; }

        [StringLength(4000)]
        public string valor_string { get; set; }

        public int? valor_entero { get; set; }

        public decimal? valor_decimal { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? valor_tiempo { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public virtual subcomponente subcomponente { get; set; }

        public virtual subcomponente_propiedad subcomponente_propiedad { get; set; }
    }
}
