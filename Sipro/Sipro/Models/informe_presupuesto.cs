namespace Sipro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sipro.informe_presupuesto")]
    public partial class informe_presupuesto
    {
        public int id { get; set; }

        public int id_prestamo { get; set; }

        public int? tipo_presupuesto { get; set; }

        public int objeto_tipo_id { get; set; }

        public int objeto_tipo { get; set; }

        public int? posicion_arbol { get; set; }

        [StringLength(1000)]
        public string objeto_nombre { get; set; }

        public int id_predecesor { get; set; }

        public int objeto_tipo_predecesor { get; set; }

        public decimal? mes1 { get; set; }

        public decimal? mes2 { get; set; }

        public decimal? mes3 { get; set; }

        public decimal? mes4 { get; set; }

        public decimal? mes5 { get; set; }

        public decimal? mes6 { get; set; }

        public decimal? mes7 { get; set; }

        public decimal? mes8 { get; set; }

        public decimal? mes9 { get; set; }

        public decimal? mes10 { get; set; }

        public decimal? mes11 { get; set; }

        public decimal? mes12 { get; set; }

        public decimal? total { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? anio { get; set; }

        [StringLength(30)]
        public string usuario_creo { get; set; }

        [StringLength(30)]
        public string usuario_actualizo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? fecha_actualizacion { get; set; }

        public int? estado { get; set; }

        public virtual estado_informe estado_informe { get; set; }
    }
}
