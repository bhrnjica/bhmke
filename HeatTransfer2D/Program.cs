using mke_core;
using System;
using System.Collections.Generic;

namespace HeatTransfer2D
{
    class Program
    {
        static void Main(string[] args)
        {
            //dimension of rectangle (domain) 
            float a, b, c, d;
            a = 12; b = 120;c = 15; d = 100;
            //nonlinear factor for meshing from y axis to the end of rectangle in horizontal dimension
            float k;
            k = 0.8f;

            //number of horizontal nodes
            int nh = 5;
            //number of vertical nodes
            int nv = 5;


            var a_parts = MeshGeneration.divideLength(a, nh, k);
            var b_parts = MeshGeneration.divideLength(b, nv, 1);
            var c_parts = MeshGeneration.divideLength(c, nh, k);
            var d_parts = MeshGeneration.divideLength(d, nv, 1);

            ElasticNode n = new ElasticNode(MKENodeType.t);
            n.x1 = 1;
            n.x2 = 1;
            n.id = 1;


            Console.Read();

        }

        
    }
}
