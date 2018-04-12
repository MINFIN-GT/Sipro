namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.objeto_prestamo")]
    public partial class objeto_prestamo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int prestamoid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int objeto_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int objeto_tipo { get; set; }

        [StringLength(30)]
        public string usuario_creo { get; set; }

        public int? usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int? estado { get; set; }

        public virtual prestamo prestamo { get; set; }
    }
}
