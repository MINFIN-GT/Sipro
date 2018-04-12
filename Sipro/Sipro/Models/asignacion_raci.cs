namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.asignacion_raci")]
    public partial class asignacion_raci
    {
        public int id { get; set; }

        public int colaboradorid { get; set; }

        [Required]
        [StringLength(1)]
        public string rol_raci { get; set; }

        public int objeto_id { get; set; }

        public int objeto_tipo { get; set; }

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

        public virtual colaborador colaborador { get; set; }
    }
}
