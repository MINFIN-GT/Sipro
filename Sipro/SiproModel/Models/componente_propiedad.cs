namespace SiproModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.componente_propiedad")]
    public partial class componente_propiedad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public componente_propiedad()
        {
            componente_propiedad_valor = new HashSet<componente_propiedad_valor>();
            ctipo_propiedad = new HashSet<ctipo_propiedad>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(1000)]
        public string nombre { get; set; }

        [StringLength(2000)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int dato_tipoid { get; set; }

        public int estado { get; set; }

        public virtual dato_tipo dato_tipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<componente_propiedad_valor> componente_propiedad_valor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ctipo_propiedad> ctipo_propiedad { get; set; }
    }
}
