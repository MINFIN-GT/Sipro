namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.estado")]
    public partial class estado
    {
        public int id { get; set; }

        [Required]
        [StringLength(30)]
        public string nombre { get; set; }

        public int valor { get; set; }
    }
}
