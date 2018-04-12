namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.desembolso")]
    public partial class desembolso
    {
        public int id { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha { get; set; }

        public int estado { get; set; }

        public decimal monto { get; set; }

        public decimal tipo_cambio { get; set; }

        public int? monto_moneda_origen { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int proyectoid { get; set; }

        public int desembolso_tipoid { get; set; }

        public int tipo_monedaid { get; set; }

        public virtual proyecto proyecto { get; set; }

        public virtual desembolso_tipo desembolso_tipo { get; set; }

        public virtual tipo_moneda tipo_moneda { get; set; }
    }
}
