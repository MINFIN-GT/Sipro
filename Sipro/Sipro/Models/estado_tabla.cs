namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.estado_tabla")]
    public partial class estado_tabla
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string tabla { get; set; }

        [Required]
        [StringLength(1000)]
        public string valores { get; set; }
    }
}
