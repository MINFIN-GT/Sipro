namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.pago_planificado")]
    public partial class pago_planificado
    {
        public int id { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_pago { get; set; }

        public decimal pago { get; set; }

        public int objeto_id { get; set; }

        public int objeto_tipo { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int estado { get; set; }
    }
}
