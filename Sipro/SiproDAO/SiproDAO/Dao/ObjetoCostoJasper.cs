using System;
using System.Collections.Generic;
using System.Text;

namespace SiproDAO.Dao
{
    public class ObjetoCostoJasper
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
        public virtual int acumulacion_costoid { get; set; }
        public virtual decimal costo { get; set; }
        public virtual decimal totalPagos { get; set; }
        public virtual int programa { get; set; }
        public virtual int subprograma { get; set; }
        public virtual int proyecto { get; set; }
        public virtual int actividad { get; set; }
        public virtual int obra { get; set; }
        public virtual int renglon { get; set; }
        public virtual int geografico { get; set; }
        public virtual String treePath { get; set; }

        public decimal eneroP = decimal.Zero;
        public decimal febreroP = decimal.Zero;
        public decimal marzoP = decimal.Zero;
        public decimal abrilP = decimal.Zero;
        public decimal mayoP = decimal.Zero;
        public decimal junioP = decimal.Zero;
        public decimal julioP = decimal.Zero;
        public decimal agostoP = decimal.Zero;
        public decimal septiembreP = decimal.Zero;
        public decimal octubreP = decimal.Zero;
        public decimal noviembreP = decimal.Zero;
        public decimal diciembreP = decimal.Zero;

        public decimal eneroR = decimal.Zero;
        public decimal febreroR = decimal.Zero;
        public decimal marzoR = decimal.Zero;
        public decimal abrilR = decimal.Zero;
        public decimal mayoR = decimal.Zero;
        public decimal junioR = decimal.Zero;
        public decimal julioR = decimal.Zero;
        public decimal agostoR = decimal.Zero;
        public decimal septiembreR = decimal.Zero;
        public decimal octubreR = decimal.Zero;
        public decimal noviembreR = decimal.Zero;
        public decimal diciembreR = decimal.Zero;

        public decimal totalP = decimal.Zero;
        public decimal totalR = decimal.Zero;

        public decimal ejecutado = decimal.Zero;
        public decimal asignado = decimal.Zero;
        public decimal modificaciones = decimal.Zero;

        int avance_fisico = 0;
        int inversion_nueva;

        public ObjetoCostoJasper(String nombre, int objeto_id, int objeto_tipo, int nivel, DateTime fecha_inicial,
            DateTime fecha_final, DateTime fecha_inicial_real, DateTime fecha_final_real, int duracion,
            int acumulacion_costoid, decimal costo, decimal totalPagos,
            int programa, int subprograma, int proyecto, int actividad, int obra, int renglon,
            int geografico, String treePath, decimal? eneroP, decimal? febreroP, decimal? marzoP,
            decimal? abrilP, decimal? mayoP, decimal? junioP, decimal? julioP, decimal? agostoP,
            decimal? septiembreP, decimal? octubreP, decimal? noviembreP, decimal? diciembreP,
            decimal? eneroR, decimal? febreroR, decimal? marzoR, decimal? abrilR, decimal? mayoR,
            decimal? junioR, decimal? julioR, decimal? agostoR, decimal? septiembreR, decimal? octubreR,
            decimal? noviembreR, decimal? diciembreR, decimal? ejecutado, decimal? asignado, decimal? modificaciones, int avance_fisico, int inversion_nueva)
        {
            this.nombre = nombre;
            this.objeto_id = objeto_id;
            this.objeto_tipo = objeto_tipo;
            this.nivel = nivel;
            this.fecha_inicial = fecha_inicial;
            this.fecha_final = fecha_final;
            this.fecha_inicial_real = fecha_inicial_real;
            this.fecha_final_real = fecha_final_real;
            this.duracion = duracion;
            this.acumulacion_costoid = acumulacion_costoid;
            this.costo = costo;
            this.totalPagos = totalPagos;
            this.programa = programa;
            this.subprograma = subprograma;
            this.proyecto = proyecto;
            this.actividad = actividad;
            this.obra = obra;
            this.renglon = renglon;
            this.geografico = geografico;
            this.treePath = treePath;
            this.inversion_nueva = inversion_nueva;
            this.eneroP = eneroP ?? default(decimal);
            this.febreroP = febreroP ?? default(decimal);
            this.marzoP = marzoP ?? default(decimal);
            this.abrilP = abrilP ?? default(decimal);
            this.mayoP = mayoP ?? default(decimal);
            this.junioP = junioP ?? default(decimal);
            this.julioP = julioP ?? default(decimal);
            this.agostoP = agostoP ?? default(decimal);
            this.septiembreP = septiembreP ?? default(decimal);
            this.octubreP = octubreP ?? default(decimal);
            this.noviembreP = noviembreP ?? default(decimal);
            this.diciembreP = diciembreP ?? default(decimal);
            this.eneroR = eneroR ?? default(decimal);
            this.febreroR = febreroR ?? default(decimal);
            this.marzoR = marzoR ?? default(decimal);
            this.abrilR = abrilR ?? default(decimal);
            this.mayoR = mayoR ?? default(decimal);
            this.junioR = junioR ?? default(decimal);
            this.julioR = julioR ?? default(decimal);
            this.agostoR = agostoR ?? default(decimal);
            this.septiembreR = septiembreR ?? default(decimal);
            this.octubreR = octubreR ?? default(decimal);
            this.noviembreR = noviembreR ?? default(decimal);
            this.diciembreR = diciembreR ?? default(decimal);

            this.totalP = this.eneroP + this.febreroP + this.marzoP + this.abrilP + this.mayoP + this.junioP + this.julioP + this.agostoP + this.septiembreP + this.octubreP + this.noviembreP + this.diciembreP;
            this.totalR = this.eneroR + this.febreroR + this.marzoR + this.abrilR + this.mayoR + this.junioR + this.julioR + this.agostoR + this.septiembreR + this.octubreR + this.noviembreR + this.diciembreR;

            this.ejecutado = ejecutado ?? default(decimal);
            this.asignado = asignado ?? default(decimal);
            this.modificaciones = modificaciones ?? default(decimal);
        }
    }
}
