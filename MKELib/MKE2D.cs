using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKELib
{
    public delegate List<MKEFElement> Problem(List<MKENode> nodes, ref double E, ref double nu);
    /// <summary>
    /// Class for defining 2D Finite Element Problem
    /// </summary>
    public class MKE2D
    {
        double E = 2.0e+11;
        double nu = 0.25;

        /// <summary>
        /// Solve 2D stress/strain problem usin Finite element method
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        public Matrix Solve(Problem init)
        {
            List<MKENode> nodes = new List<MKENode>();
            List<MKEFElement> eColl = init(nodes, ref E, ref nu);

            //Global stiffness m atrix
            Matrix gs = calculateGlobalStiffness(nodes, eColl, E, nu);

            //Apply BC
            Matrix u = solveMKE(gs, nodes);

            //Solve System of equation
            return u;
        }

        /// <summary>
        /// from the boundary condition and global stifness matrix calculate unknown displacements
        /// </summary>
        /// <param name="gs">global stiffness matrix</param>
        /// <param name="nodes">defined nodes</param>
        /// <returns></returns>
        private Matrix solveMKE(Matrix gs, List<MKENode> nodes)
        {
            int count = getMatrixStiffOrder(nodes);
            Matrix f = new Matrix(count, 4);
            for (int i = 0; i < nodes.Count; i++)
            {
                //set extenal loads vektor
                int ind = i * nodes[i].GetDof();
                //Index columns of the f matrix
                //0 - nodeID, 1 - nodetype, 2 - loads, 3-displacements
                setExternalLoads(f, nodes[i], ind);
            }

            //caclulate number of unknowns of the system
            var sysCount = f.GetCol(2).mat.Where(x => !double.IsNaN(x)).Count();
            Matrix A = new Matrix(sysCount, sysCount);
            Matrix b = new Matrix(sysCount, 1);

            //create system equation matrix
            int k = 0;
            int l = 0;
            for (int i = 0; i < count; i++)
            {
                if (!double.IsNaN(f[i, 2]))
                {
                    b[k, 0] = f[i, 2];
                    l = 0;
                    //
                    for (int j = 0; j < count; j++)
                    {
                        if (!double.IsNaN(f[j, 2]))
                        {
                            A[k, l] = gs[i, j];

                            //increase col index
                            l++;
                        }
                    }
                    //increase row index
                    k++;
                }
            }

           //calculate unknown displacements
           var u = A.SolveWith(b);

            //format calculated displacement for printing
            int index = 0;
            for (int i= 0; i < count; i++)
            {
                if (double.IsNaN(f[i, 3]))
                {
                    f[i, 3] = u[index,0];
                    index++;
                }      
            }

            //return suitable format of calculated displacements and loads
            return f;
        }

        /// <summary>
        /// Calculate Degree of freedom for global stiffness matrix
        /// </summary>
        /// <param name="nodes">nodes</param>
        /// <returns></returns>
        private int getMatrixStiffOrder(List<MKENode> nodes)
        {
            return nodes.Count() * nodes[0].GetDof();
        }

        /// <summary>
        /// calculate global stifness matri
        /// </summary>
        /// <param name="nodes">defined nodes</param>
        /// <param name="eColl">finite element coletions</param>
        /// <param name="E">MOdul of Elasticity</param>
        /// <param name="nu">Paisson coefficient</param>
        /// <returns></returns>
        private Matrix calculateGlobalStiffness(List<MKENode> nodes, List<MKEFElement> eColl, double E, double nu)
        {
            //Calculate stiffness matrices for each finite element
            foreach (var e in eColl)
                e.calStiffness(E, nu);

            //calculate number of row/cols of the global strifness matrix
            int iOrder = nodes.Count() * nodes[0].GetDof();
            //
            Matrix gS = new Matrix(iOrder, iOrder);

            //global stiffness matrix calculation
            //foreach node in row direction
            for (int i = 0; i < nodes.Count; i++)
            {
                //mark row node ID
                var rowNodeId = nodes[i].id;

                //foreach node in column direction
                for (int j = 0; j < nodes.Count; j++)
                {
                    //mark column node Id
                    var colNodeId = nodes[j].id;

                    //for each finite element
                    foreach (var e in eColl)
                    {
                        //for each node of curent finite element
                        for (int k = 0; k < e.nodes.Count; k++)
                        {
                            //find node with specified row node Id
                            if (e.nodes[k].id == rowNodeId)
                            {
                                //for each node of curent finite element
                                for (int l = 0; l < e.nodes.Count; l++)
                                {
                                    //find node with specified column node Id
                                    if (e.nodes[l].id == colNodeId)
                                    {
                                        //for each node degree of freedom in row direction
                                        for (int m = 0; m < e.nodes[k].GetDof(); m++)
                                        {
                                            //for each node degree of freedom in column direction
                                            for (int n = 0; n < e.nodes[l].GetDof(); n++)
                                            {
                                                //calculate global stiffness matrix element value
                                                int row = i * nodes[k].GetDof() + m;
                                                int col = j * nodes[n].GetDof() + n;
                                                int ki = k * nodes[k].GetDof() + m;
                                                int lj = l * nodes[n].GetDof() + n;
                                                //
                                                gS[row, col] = gS[row, col] + e.kStiffness[ki, lj];

                                            }
                                        }

                                    }
                                }

                            }
                        }

                    }
                }

            }

            return gS;
        }

        /// <summary>
        /// analizes and prepare external loads for system of equations
        /// </summary>
        /// <param name="f"> matix of node loads</param>
        /// <param name="nodes">nodes</param>
        /// <param name="index">collection index</param>
        private void setExternalLoads(Matrix f, MKENode nodes, int index)
        {
            if (nodes.type == MKENodeType.uv)
            {
                //node id
                f[index, 0] = nodes.id;
                f[index + 1, 0] = nodes.id;

                //node type
                f[index, 1] = (int)nodes.type;
                f[index + 1, 1] = (int)nodes.type;

                //loads
                f[index, 2] = nodes.fx;              
                f[index + 1, 2] = nodes.fy;

                //displacements
                f[index, 3] = nodes.u;
                f[index + 1, 3] = nodes.v;

            }
            else
                throw new Exception("Not supported node type!");
        }
    }
}
