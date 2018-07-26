using net.sf.mpxj;
using net.sf.mpxj.mpp;
using net.sf.mpxj.reader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    class CProject
    {
        ProjectFile project;
        ProjectReader reader;

        public CProject(String nombre)
        {
            try
            {
                reader = new MPPReader();
                project = reader.read(nombre);
            }
            catch (Exception e)
            {
                CLogger.write("1", "CProject.class", e);
            }
        }
    }
}
