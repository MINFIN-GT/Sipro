namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.componente_sigade")]
    public partial class componente_sigade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public componente_sigade()
        {
            componente = new HashSet<componente>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(2000)]
        public string nombre { get; set; }

        [Required]
        [StringLength(45)]
        public string codigo_presupuestario { get; set; }

        public int numero_componente { get; set; }

        public decimal monto_componente { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(30)]
        public string usuaraio_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<componente> componente { get; set; }
    }
}
