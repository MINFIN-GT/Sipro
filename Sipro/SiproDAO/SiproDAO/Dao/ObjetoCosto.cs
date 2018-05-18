using System;
using System.Collections.Generic;
using System.Text;

namespace SiproDAO.Dao
{
    public class ObjetoCosto
    {
        public virtual String nombre { get; set; }
        public virtual int objeto_id { get; set; }
        public virtual int objeto_tipo { get; set; }
        public virtual int nivel { get; set; }
        public virtual DateTime fecha_inicial { get; set; }
        public virtual DateTime fecha_final { get; set; }
        public virtual DateTime fecha_inicial_real { get; set; }
        public virtual DateTime fecha_final_real { get; set; }
        public virtual int duracion { get; set; }
        public virtual stanio[] anios { get; set; }
        public virtual int acumulacion_costoid { get; set; }
        public virtual decimal costo { get; set; }
        public virtual decimal totalPagos { get; set; }
        public virtual int unidad_ejecutora { get; set; }
        public virtual int entidad { get; set; }
        public virtual int programa { get; set; }
        public virtual int subprograma { get; set; }
        public virtual int proyecto { get; set; }
        public virtual int actividad { get; set; }
        public virtual int obra { get; set; }
        public virtual int renglon { get; set; }
        public virtual int geografico { get; set; }
        public virtual String treePath { get; set; }
        public decimal ejecutado = new decimal(0);
        public decimal asignado = new decimal(0);
        public decimal modificaciones = new decimal(0);
        public int avance_fisico = 0;
        public int inversion_nueva = 0;
        public virtual ObjetoCosto parent { get; set; }
        public virtual List<ObjetoCosto> children { get; set; }

        public void inicializarStanio(int anioInicial, int anioFinal)
        {
            int longitudArrelgo = anioFinal - anioInicial + 1;

            anios = new stanio[longitudArrelgo];

            for (int i = 0; i < longitudArrelgo; i++)
            {
                stanio temp = new stanio();
                for (int m = 0; m < 12; m++)
                {
                    temp.mes[m] = new stpresupuesto();
                    temp.mes[m].planificado = default(decimal);
                    temp.mes[m].real = default(decimal);
                }
                temp.anio = anioInicial + i;
                anios[i] = temp;
            }
        }

        public List<ObjetoCosto> getListado(ObjetoCosto nodo)
        {
            List<ObjetoCosto> lstPrestamo = new List<ObjetoCosto>();
            lstPrestamo.Add(nodo);
            if (nodo.children != null && nodo.children.Count != 0)
            {
                for (int h = 0; h < nodo.children.Count; h++)
                {
                    lstPrestamo.AddRange((List<ObjetoCosto>)getListado(nodo.children[h]));
                }
            }
            return lstPrestamo;
        }

        public class stpresupuesto
        {
            public decimal planificado;
            public decimal real;
        }

        public class stanio
        {
            public stpresupuesto[] mes = new stpresupuesto[12];
            public int anio;
        }
    }
}
