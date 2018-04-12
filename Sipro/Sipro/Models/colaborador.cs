namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.colaborador")]
    public partial class colaborador
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public colaborador()
        {
            asignacion_raci = new HashSet<asignacion_raci>();
            proyecto_rol_colaborador = new HashSet<proyecto_rol_colaborador>();
            proyecto_miembro = new HashSet<proyecto_miembro>();
            proyecto = new HashSet<proyecto>();
            riesgo = new HashSet<riesgo>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string pnombre { get; set; }

        [StringLength(255)]
        public string snombre { get; set; }

        [Required]
        [StringLength(255)]
        public string papellido { get; set; }

        [StringLength(255)]
        public string sapellido { get; set; }

        public long cui { get; set; }

        public int unidad_ejecutoraunidad_ejecutora { get; set; }

        [StringLength(30)]
        public string usuariousuario { get; set; }

        public int estado { get; set; }

        [Required]
        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int entidad { get; set; }

        public int ejercicio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<asignacion_raci> asignacion_raci { get; set; }

        public virtual unidad_ejecutora unidad_ejecutora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_rol_colaborador> proyecto_rol_colaborador { get; set; }

        public virtual usuario usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto_miembro> proyecto_miembro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto> proyecto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<riesgo> riesgo { get; set; }
    }
}
