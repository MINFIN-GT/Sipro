namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.proyecto_rol_colaborador")]
    public partial class proyecto_rol_colaborador
    {
        public int id { get; set; }

        public int colaboradorid { get; set; }

        public int proyectoid { get; set; }

        public int rol_unidad_ejecutoraid { get; set; }

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

        public virtual proyecto proyecto { get; set; }

        public virtual rol_unidad_ejecutora rol_unidad_ejecutora { get; set; }
    }
}
