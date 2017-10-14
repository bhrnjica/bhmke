using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mke_core
{
    public delegate List<EFElement> Problem(List<ElasticNode> nodes, ref double E, ref double nu, ref double t);
    /// <summary>
    /// Class for defining 2D Finite Element Problem
    /// </summary>
    public class FeProblem2d
    {
        double E = 2.0e+11;
        double nu = 0.25;
        double t = 1;//thickness 
        StreamWriter stream = null;

        public FeProblem2d(StreamWriter s = null)
        {
            stream = s;
        }
        /// <summary>
        /// Solve 2D stress/strain problem using Finite element method
        /// </summary>
        /// <param name="init"></param>
        /// <param name="resultFilePAth"> file path for result output</param>
        /// <returns></returns>
        public Matrix Solve(Problem init)
        {
            //call callback function to initialize the problem
            var nodes = new List<ElasticNode>();
            List<EFElement> eColl = init(nodes, ref E, ref nu, ref t);

            //Global stiffness m matrix
            Matrix gs = calculateGlobalStiffness(nodes, eColl, E, nu, t);

            //write global matrix
            if (stream != null)
            {
                outputStifness(gs);
            }

            //Apply BC
            Matrix u = solveMKE(gs, nodes);


            //Solve System of equation
            return u;
        }

        private void outputStifness(Matrix gs)
        {
            if (stream != null)
            {
                stream.WriteLine("******************START ** 2D SOLVER ** START******************");
                stream.WriteLine("******* Global Stiffness matrix***********");
                stream.WriteLine("{0}", gs.ToString());
                stream.WriteLine(" ");
            }
        }

        /// <summary>
        /// from the boundary condition and global stiffness matrix calculate unknown displacements
        /// </summary>
        /// <param name="gs">global stiffness matrix</param>
        /// <param name="nodes">defined nodes</param>
        /// <returns></returns>
        private Matrix solveMKE(Matrix gs, List<ElasticNode> nodes)
        {
            int count = getMatrixStiffOrder(nodes);
            Matrix f = new Matrix(count, 4);
            for (int i = 0; i < nodes.Count; i++)
            {
                //set external loads vector
                int ind = i * nodes[i].GetDof();
                //Index columns of the f matrix
                //0 - nodeID, 1 - node type, 2 - loads, 3-displacements
                setExternalLoads(f, nodes[i], ind);
            }

            //calculate number of unknowns of the system
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
            for (int i = 0; i < count; i++)
            {
                if (double.IsNaN(f[i, 3]))
                {
                    f[i, 3] = u[index, 0];
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
        private int getMatrixStiffOrder(List<ElasticNode> nodes)
        {
            return nodes.Count() * nodes[0].GetDof();
        }

        /// <summary>
        /// calculate global stiffness matrix
        /// </summary>
        /// <param name="nodes">defined nodes</param>
        /// <param name="eColl">finite element collections</param>
        /// <param name="E">MOdul of Elasticity</param>
        /// <param name="nu">Poisson coefficient</param>
        /// <returns></returns>
        private Matrix calculateGlobalStiffness(List<ElasticNode> nodes, List<EFElement> eColl, double E, double nu, double t)
        {
            //Calculate stiffness matrices for each finite element
            foreach (var e in eColl)
            {
                e.calcStiffness(E, nu, t);
                //output results for each finite element
                if (stream != null)
                    e.WriteMatrices(stream);
            }


            //calculate number of row/cols of the global strictness matrix
            int iOrder = nodes.Count() * nodes[0].GetDof();
            //
            Matrix gS = new Matrix(iOrder, iOrder);

            //global stiffness matrix calculation
            //for each node in row direction
            for (int i = 0; i < nodes.Count; i++)
            {
                //mark row node ID
                var rowNodeId = nodes[i].id;

                //for each node in column direction
                for (int j = 0; j < nodes.Count; j++)
                {
                    //mark column node Id
                    var colNodeId = nodes[j].id;

                    //for each finite element
                    foreach (var e in eColl)
                    {
                        //for each node of current finite element
                        for (int k = 0; k < e.nodes.Count; k++)
                        {
                            //find node with specified row node Id
                            if (e.nodes[k].id == rowNodeId)
                            {
                                //for each node of current finite element
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
        /// analyzes and prepare external loads for system of equations
        /// </summary>
        /// <param name="f"> matrix of node loads</param>
        /// <param name="nodes">nodes</param>
        /// <param name="index">collection index</param>
        private void setExternalLoads(Matrix f, ElasticNode nodes, int index)
        {
            if (nodes.Type == MKENodeType.uv)
            {
                //node id
                f[index, 0] = nodes.id;
                f[index + 1, 0] = nodes.id;

                //node type
                f[index, 1] = (int)nodes.Type;
                f[index + 1, 1] = (int)nodes.Type;

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
