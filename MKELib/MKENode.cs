using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKELib
{
    /// <summary>
    /// Clas provides basic functionality for node 
    /// </summary>
    public class MKENode
    { 
        //identification       
        public int id;
        public MKENodeType type;

        //coordinate
        public double x;
        public double y;

        //displacements
        public double u;
        public double v;
        public double fiu;
        public double fiv;

        //loads
        public double fx;
        public double fy;
        public double mxz;
        public double myz;

        //
        public MKENode(MKENodeType t= MKENodeType.uv)
        {
            id = 0;
            type = t;
            x = y = u = v = fiu = fiv = fx = fy = mxz = myz = double.NaN;
        }

        public int GetDof()
        {
            if (type == MKENodeType.uv)
                return 2;
            else if (type == MKENodeType.uvuf)
                return 3;
            else if (type == MKENodeType.uvvf)
                return 3;
            else if (type == MKENodeType.uvufvf)
                return 4;
            else 
                throw new Exception("Not suported node type!");

        }
    }
}
