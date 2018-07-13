using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mke_core
{
    /// <summary>
    /// Class provides basic functionality for node of heat transfer
    /// </summary>
    public class ThermoNode : NodeBase
    {
        

        //unknown variable
        public double t;
        
        //
        public double fx;
        
        //
        public ThermoNode(MKENodeType type = MKENodeType.t)
        {
            id = 0;
            Type = type;
            x1 = x2 = t = double.NaN;
        }

        public override int GetDof()
        {
            return 1;
        }
    }
}
