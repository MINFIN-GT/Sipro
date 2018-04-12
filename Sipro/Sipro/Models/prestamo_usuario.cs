namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.prestamo_usuario")]
    public partial class prestamo_usuario
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int prestamoid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string usuario { get; set; }

        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public virtual prestamo prestamo { get; set; }

        public virtual usuario usuario1 { get; set; }
    }
}
