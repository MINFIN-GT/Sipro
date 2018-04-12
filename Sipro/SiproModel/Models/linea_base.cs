namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.linea_base")]
    public partial class linea_base
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        public int proyectoid { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int tipo_linea_base { get; set; }

        public int? sobreescribir { get; set; }

        public virtual proyecto proyecto { get; set; }
    }
}
