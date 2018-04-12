namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.plan_adquisicion_pago")]
    public partial class plan_adquisicion_pago
    {
        public int id { get; set; }

        public int plan_adquisicionid { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_pago { get; set; }

        public decimal? pago { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int? estado { get; set; }

        public virtual plan_adquisicion plan_adquisicion { get; set; }
    }
}
