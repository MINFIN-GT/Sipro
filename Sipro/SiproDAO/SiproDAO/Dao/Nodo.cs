using System;
using System.Collections.Generic;
using System.Text;

namespace SiproDAO.Dao
{
    public partial class Nodo
    {
        public virtual int id { get; set; }
        public virtual int objeto_tipo { get; set; }
        public virtual String nombre { get; set; }
        public virtual int nivel { get; set; }
        public virtual bool estado { get; set; }
        public virtual List<Nodo> children { get; set; }
        public virtual Nodo parent { get; set; }
        public virtual DateTime fecha_inicio { get; set; }
        public virtual DateTime fecha_fin { get; set; }
        public virtual int duracion { get; set; }
        public virtual decimal costo { get; set; }
        public virtual Object objeto { get; set; }
        public virtual DateTime fecha_inicio_real { get; set; }
        public virtual DateTime fecha_fin_real { get; set; }
    }
}
