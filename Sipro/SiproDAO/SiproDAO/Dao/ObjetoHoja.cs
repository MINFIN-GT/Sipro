using System;
using System.Collections.Generic;
using System.Text;

namespace SiproDAO.Dao
{
    public class ObjetoHoja
    {
        int objetoTipoHoja;
        int objetoTipoPadre;
        Object padre;
        Object hoja;

        public ObjetoHoja(int objetoTipoHoja, Object hoja, int objetoTipoPadre, Object padre)
        {
            this.objetoTipoHoja = objetoTipoHoja;
            this.hoja = hoja;
            this.objetoTipoPadre = objetoTipoPadre;
            this.padre = padre;
        }
    }
}
