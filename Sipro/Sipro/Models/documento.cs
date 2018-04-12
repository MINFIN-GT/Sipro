namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.documento")]
    public partial class documento
    {
        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [Required]
        [StringLength(45)]
        public string extension { get; set; }

        public int id_tipo_objeto { get; set; }

        public int id_objeto { get; set; }

        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int? estado { get; set; }
    }
}
