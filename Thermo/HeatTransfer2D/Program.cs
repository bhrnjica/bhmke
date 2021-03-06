﻿using mke_core;
using System;
using System.Collections.Generic;
using System.IO;

namespace HeatTransfer2D
{
    class Program
    {
        

        static void Main(string[] args)
        {
            //prepare file for logging
            var fileName = @"C:/Users/BahrudinHrnjica/Desktop/thermo.txt";
            var logFile = System.IO.File.Create(fileName);
            var logWriter = new System.IO.StreamWriter(logFile);

            ///****************INPUT******************///
            //dimension of rectangle (domain) 
            float a, b;         //, c, d;
            a = 20; b = 120;    //c = 15; d = 100;
            //nonlinear factor for meshing from y axis to the end of rectangle in horizontal dimension
            float k;
            k = 0.8f;

            //number of horizontal nodes
            int nh = 40;
            //number of vertical nodes
            int nv = 70;


            var a_parts = MeshGeneration.divideLength(a, nh, k);
            var b_parts = MeshGeneration.divideLength(b, nv, 1);
            //var c_parts = MeshGeneration.divideLength(c, nh, k);
            //var d_parts = MeshGeneration.divideLength(d, nv, 1);

            ///node generation
            ThermoNode[][] nodes = MeshGeneration.GenerateNodes(a_parts,b_parts);

            //print nodes
            using (StreamWriter tw = new StreamWriter(logWriter.BaseStream))
            {
                MeshGeneration.printNodes(nodes, tw);
            }

            //finite element generation
            List<HeatFElement> els = MeshGeneration.GenerateElements(nodes);

            //

            // Console.Read();

        }

       
    }
}
