namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.formulario_item")]
    public partial class formulario_item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public formulario_item()
        {
            formulario_item_valor = new HashSet<formulario_item_valor>();
            formulario_item_opcion = new HashSet<formulario_item_opcion>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(4000)]
        public string texto { get; set; }

        public int formularioid { get; set; }

        public int orden { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        public int? usuario_actualizacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int estado { get; set; }

        public int formulario_item_tipoid { get; set; }

        public virtual formulario formulario { get; set; }

        public virtual formulario_item_tipo formulario_item_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<formulario_item_valor> formulario_item_valor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<formulario_item_opcion> formulario_item_opcion { get; set; }
    }
}
