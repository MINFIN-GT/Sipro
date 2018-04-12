namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.proyecto_impacto")]
    public partial class proyecto_impacto
    {
        public int id { get; set; }

        public int proyectoid { get; set; }

        public int entidadentidad { get; set; }

        [Required]
        [StringLength(1000)]
        public string impacto { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int ejercicio { get; set; }

        public virtual entidad entidad { get; set; }

        public virtual proyecto proyecto { get; set; }
    }
}
