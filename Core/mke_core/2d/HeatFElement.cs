using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mke_core
{
    /// <summary>
    /// class implements finite element for heat transfer
    /// </summary>
    public class HeatFElement : FElementBase
    {

        public double bValue;
        public Matrix bMatrix;
        public double eValue;
        public Matrix eMatrix;
        public double tArea;//are of the triangle
        public Matrix kStiffness;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="elemId"></param>
        /// <param name="plane"></param>
        public HeatFElement(int elemId = 0, MKEElementType t = MKEElementType.quad)
        {
            id = elemId;
            type = t;
            nodes = new List<NodeBase>();
        }


        internal void WriteMatrices(StreamWriter tw)
        {
            if (tw != null)
            {
                tw.WriteLine("******************START **  FE_ID={0}  ** START******************", id);
                tw.WriteLine("Type={0}", type);
                tw.WriteLine("A={0}", tArea);
                //nodes
                tw.Write("Nodes:(ID; X; Y)={");
                foreach (var n in nodes)
                {
                    tw.Write("({0}; {1}; {2})", n.id, n.x1, n.x2);
                }
                tw.Write("}");

                //B Matrix
                tw.WriteLine(" ");
                tw.WriteLine("*******B Matrix*****");
                tw.WriteLine(" ");
                tw.WriteLine("B={0}* [", bValue);
                tw.WriteLine("{0}", bMatrix.ToString());
                tw.WriteLine("]");

                //E matrix
                tw.WriteLine("*******E Matrix*****");
                tw.WriteLine("");
                tw.WriteLine("E={0}* [", eValue);
                tw.WriteLine("{0}", eMatrix.ToString());
                tw.WriteLine("]");

                //Stiffness matrix
                tw.WriteLine("*******Stiffness Matrix*****");
                tw.WriteLine("");
                tw.WriteLine("{0}", kStiffness.ToString());
                //End of write
                tw.WriteLine("******************END **  FE_ID={0}  ** END******************", id);
                tw.WriteLine("");
                tw.WriteLine("");
            }

        }
    }
}
