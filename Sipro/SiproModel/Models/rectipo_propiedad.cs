namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.rectipo_propiedad")]
    public partial class rectipo_propiedad
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int recurso_propiedadid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int recurso_tipoid { get; set; }

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

        public virtual recurso_tipo recurso_tipo { get; set; }

        public virtual recurso_propiedad recurso_propiedad { get; set; }
    }
}
