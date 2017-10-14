using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mke_core
{
    /// <summary>
    /// Class provides basic functionality for node 
    /// </summary>
    public class ElasticNode : NodeBase
    {
        

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
        public ElasticNode(MKENodeType t = MKENodeType.uv)
        {
            id = 0;
            Type = t;
            x1 = x2 = u = v = fiu = fiv = fx = fy = mxz = myz = double.NaN;
        }

        public override int GetDof()
        {
            if (Type == MKENodeType.uv)
                return 2;
            else if (Type == MKENodeType.uvuf)
                return 3;
            else if (Type == MKENodeType.uvvf)
                return 3;
            else if (Type == MKENodeType.uvufvf)
                return 4;
            else
                throw new Exception("Not supported node type!");

        }
    }
}
