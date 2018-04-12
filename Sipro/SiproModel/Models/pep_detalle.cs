namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.pep_detalle")]
    public partial class pep_detalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int proyectoid { get; set; }

        [StringLength(4000)]
        public string observaciones { get; set; }

        [StringLength(4000)]
        public string alertivos { get; set; }

        [StringLength(100)]
        public string elaborado { get; set; }

        [StringLength(100)]
        public string aprobado { get; set; }

        [StringLength(100)]
        public string autoridad { get; set; }

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

        public virtual proyecto proyecto { get; set; }
    }
}
