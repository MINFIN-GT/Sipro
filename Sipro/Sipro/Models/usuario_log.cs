namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.usuario_log")]
    public partial class usuario_log
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string usuario { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "timestamp")]
        public DateTime fecha { get; set; }
    }
}
