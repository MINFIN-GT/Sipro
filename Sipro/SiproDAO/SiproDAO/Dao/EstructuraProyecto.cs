using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiproModelCore.Models
{
    public partial class EstructuraProyecto
    {
        public virtual Int32 id { get; set; }
        public virtual string nombre { get; set; }
        public virtual Int32 objeto_tipo { get; set; }
        public virtual string treePath { get; set; }
        public virtual DateTime? fecha_inicio { get; set; }
        public virtual DateTime? fecha_fin { get; set; }
        public virtual Int32 duracion { get; set; }
        public virtual string duracion_dimension { get; set; }
        public virtual decimal? costo { get; set; }
        public virtual Int32 acumulacion_costoid { get; set; }
        public virtual Int32? programa { get; set; }
        public virtual Int32? subprograma { get; set; }
        public virtual Int32? proyecto { get; set; }
        public virtual Int32? actividad { get; set; }
        public virtual Int32? obra { get; set; }
        public virtual DateTime? fecha_inicio_real { get; set; }
        public virtual DateTime? fecha_fin_real { get; set; }
        public virtual Int32? porcentaje_avance { get; set; }
        public virtual Int32 objeto_tipo_pred { get; set; }
    }
}
