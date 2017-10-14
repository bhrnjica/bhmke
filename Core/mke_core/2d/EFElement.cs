using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mke_core
{
    /// <summary>
    /// class implements elastic finite element
    /// </summary>
    public class EFElement : FElementBase
    {

        public MKEPlaneType pType;
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
        public EFElement(int elemId = 0, MKEElementType t = MKEElementType.triangle, MKEPlaneType plane = MKEPlaneType.stress)
        {
            id = elemId;
            pType = plane;
            type = t;
            nodes = new List<NodeBase>();
        }
        /// <summary>
        /// Calculate stiffness matrix from  b and e matrix
        /// </summary>
        /// <param name="E">Elasticity module</param>
        /// <param name="nu">Poisson coefficient</param>
        public void calcStiffness(double E, double nu, double t)
        {
            calBMatrix();
            calEMatrix(E, nu);
            var bT = Matrix.Transpose(bMatrix);
            var btEb1 = eMatrix * bMatrix;
            var btEb = bT * btEb1;
            var s = t * tArea * eValue / (bValue * bValue);
            kStiffness = s * btEb;

        }
        /// <summary>
        /// Calculation of the B matrix
        /// </summary>
        private void calBMatrix()
        {
            if (type == MKEElementType.triangle)
            {
                bMatrix = new Matrix(3, 6);
                double a1 = nodes[1].x1 * nodes[2].x2 - nodes[2].x1 * nodes[1].x2;
                double a2 = nodes[2].x1 * nodes[0].x2 - nodes[0].x1 * nodes[2].x2;
                double a3 = nodes[0].x1 * nodes[1].x2 - nodes[1].x1 * nodes[0].x2;
                double b1 = nodes[1].x2 - nodes[2].x2;
                double b2 = nodes[2].x2 - nodes[0].x2;
                double b3 = nodes[0].x2 - nodes[1].x2;
                double g1 = nodes[2].x1 - nodes[1].x1;
                double g2 = nodes[0].x1 - nodes[2].x1;
                double g3 = nodes[1].x1 - nodes[0].x1;
                bValue = nodes[0].x1 * (nodes[1].x2 - nodes[2].x2) + nodes[1].x1 * (nodes[2].x2 - nodes[0].x2) + nodes[2].x1 * (nodes[0].x2 - nodes[1].x2);
                tArea = 0.5 * bValue;
                bMatrix[0, 0] = b1; bMatrix[0, 1] = 0; bMatrix[0, 2] = b2; bMatrix[0, 3] = 0; bMatrix[0, 4] = b3; bMatrix[0, 5] = 0;
                bMatrix[1, 0] = 0; bMatrix[1, 1] = g1; bMatrix[1, 2] = 0; bMatrix[1, 3] = g2; bMatrix[1, 4] = 0; bMatrix[1, 5] = g3;
                bMatrix[2, 0] = g1; bMatrix[2, 1] = b1; bMatrix[2, 2] = g2; bMatrix[2, 3] = b2; bMatrix[2, 4] = g3; bMatrix[2, 5] = b3;
            }
            else
                throw new Exception("Not supported type!");
        }
        /// <summary>
        /// Calculation of E matrix
        /// </summary>
        /// <param name="E"> Modulo Elasticity</param>
        /// <param name="nu"> Poisson coefficient</param>
        private void calEMatrix(double E, double nu)
        {
            if (type == MKEElementType.triangle)
            {
                if (pType == MKEPlaneType.stress)
                {
                    eValue = E / (1.0 - nu * nu);
                    eMatrix = new Matrix(3, 3);
                    eMatrix[0, 0] = 1.0; eMatrix[0, 1] = nu; eMatrix[0, 2] = 0;
                    eMatrix[1, 0] = nu; eMatrix[1, 1] = 1.0; eMatrix[1, 2] = 0;
                    eMatrix[2, 0] = 0; eMatrix[2, 1] = 0; eMatrix[2, 2] = (1.0 - nu) / 2.0;
                }
                else//plane strain state
                {
                    eValue = E / ((1.0 + nu) * (1.0 - 2.0 * nu));
                    eMatrix = new Matrix(3, 3);
                    eMatrix[0, 0] = 1.0 - nu; eMatrix[0, 1] = nu; eMatrix[0, 2] = 0;
                    eMatrix[1, 0] = nu; eMatrix[1, 1] = 1.0 - nu; eMatrix[1, 2] = 0;
                    eMatrix[2, 0] = 0; eMatrix[2, 1] = 0; eMatrix[2, 2] = (1.0 - 2.0 * nu) / 2.0;
                }
            }
            else
                throw new Exception("Not supported type!");
        }

        internal void WriteMatrices(StreamWriter tw)
        {
            if (tw != null)
            {
                tw.WriteLine("******************START **  FE_ID={0}  ** START******************", id);
                tw.WriteLine("Type={0}", type);
                tw.WriteLine("PlaneType={0}", pType);
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
