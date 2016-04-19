using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKELib
{
    public class MKEFElement
    {
        public List<MKENode> nodes;
        public int id;
        public MKEElementType type;
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
        public MKEFElement(int elemId=0, MKEElementType t= MKEElementType.triangle, MKEPlaneType plane = MKEPlaneType.stress)
        {
            id = elemId;
            pType = plane;
            type = t;
            nodes = new List<MKENode>();
        }
        /// <summary>
        /// Calculate stifness matrix from  b and e matix
        /// </summary>
        /// <param name="E">Elasticity modul</param>
        /// <param name="nu">Poisson coeff</param>
        public void calcStiffness(double E, double nu, double t)
        {
            calBMatrix();
            calEMatrix(E,nu);
            var bT = Matrix.Transpose(bMatrix);
            var btEb1 =   eMatrix * bMatrix;
            var btEb = bT * btEb1;
            var s = t * tArea * eValue / (bValue *bValue);
            kStiffness = s * btEb;
        }
        /// <summary>
        /// Calculation of the B matrix
        /// </summary>
        private void calBMatrix()
        {
            if (type == MKEElementType.triangle)
            {
                bMatrix = new Matrix(3,6);
                double a1 = nodes[1].x * nodes[2].y - nodes[2].x * nodes[1].y;
                double a2 = nodes[2].x * nodes[0].y - nodes[0].x * nodes[2].y;
                double a3 = nodes[0].x * nodes[1].y - nodes[1].x * nodes[0].y;
                double b1 = nodes[1].y - nodes[2].y;
                double b2 = nodes[2].y - nodes[0].y;
                double b3 = nodes[0].y - nodes[1].y;
                double g1 = nodes[2].x - nodes[1].x;
                double g2 = nodes[0].x - nodes[2].x;
                double g3 = nodes[1].x - nodes[0].x;
                bValue = nodes[0].x * (nodes[1].y - nodes[2].y) + nodes[1].x * (nodes[2].y - nodes[0].y) + nodes[2].x * (nodes[0].y - nodes[1].y);
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
        /// <param name="E"> Modul Elasticity</param>
        /// <param name="nu"> Poisson coeff</param>
        private void calEMatrix(double E, double nu)
        {
            if (type == MKEElementType.triangle)
            {
                if(pType== MKEPlaneType.stress)
                {
                    eValue = E / (1.0 - nu*nu);
                    eMatrix = new Matrix(3, 3);
                    eMatrix[0, 0] = 1.0; eMatrix[0, 1] = nu; eMatrix[0, 2] = 0;
                    eMatrix[1, 0] = nu; eMatrix[1, 1] = 1.0; eMatrix[1, 2] = 0;
                    eMatrix[2, 0] = 0; eMatrix[2, 1] = 0; eMatrix[2, 2] = (1.0-nu)/2.0;
                }
                else//plane strain state
                {
                    eValue = E / ((1.0 + nu)*(1.0-2.0*nu));
                    eMatrix = new Matrix(3, 3);
                    eMatrix[0, 0] = 1.0-nu; eMatrix[0, 1] = nu; eMatrix[0, 2] = 0;
                    eMatrix[1, 0] = nu; eMatrix[1, 1] = 1.0-nu; eMatrix[1, 2] = 0;
                    eMatrix[2, 0] = 0; eMatrix[2, 1] = 0; eMatrix[2, 2] = (1.0 - 2.0*nu) / 2.0;
                }
            }
            else
                throw new Exception("Not supported type!");
        }
    }
}
