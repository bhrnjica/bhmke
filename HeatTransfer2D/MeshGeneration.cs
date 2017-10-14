using mke_core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeatTransfer2D
{
    public class MeshGeneration
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">length to be split in to n parts</param>
        /// <param name="n"> number of parts</param>
        /// <param name="k"> for k=1 all parts are the same, for k not equal to 1, parts are unequal</param>
        /// <returns></returns>
        public static float[] divideLength(float length, int n, float k)
        {
            float part = length / n;
            float sumParts = 0;
            //calculate length progression
            for (int i = 0; i < n; i++)
            {
                float currentPart = (float)(part * Math.Pow(k, i));
                sumParts += currentPart;
            }

            float progPart = (length - sumParts) / n;
            //calculate part length
            List<float> a_parts = new List<float>();
            for (int i = 0; i < n; i++)
            {
                var curPart = (float)(part * Math.Pow(k, i)) + progPart;
                a_parts.Add(curPart);
            }

            return a_parts.ToArray();
        }

        /// <summary>
        /// Generate Thermo Nodes based on parts of horizontal a and vertical b length
        /// </summary>
        /// <param name="a_parts">parts length of horizontal length</param>
        /// <param name="b_parts">parts length of vertical length</param>
        /// <returns></returns>
        internal static ThermoNode[][] GenerateNodes(float[] a_parts, float[] b_parts)
        {
            var nh = a_parts.Length;
            var nv = b_parts.Length;
            //
            var nodes = new ThermoNode[nh + 1][];
            int nIndex = 1;
            float r = 0;
            for (int i = 0; i <= nh; i++)
            {
                float z = 0;
                nodes[i] = new ThermoNode[nv + 1];

                for (int j = 0; j <= nv; j++)
                {
                    var n = new ThermoNode(MKENodeType.t);
                    n.x1 = r;
                    n.x2 = z;
                    n.id = nIndex;
                    nodes[i][j] = n;
                    nIndex++;
                    if (j < nv)
                        z += b_parts[j];
                }

                if (i < nh)
                    r += a_parts[i];
            }

            return nodes;
        }

        internal static List<HeatFElement> GenerateElements(ThermoNode[][] nodes)
        {
            var els = new List<HeatFElement>();
            int id = 1;
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                for (int j = 0; j < nodes[0].Length - 1; j++)
                {
                    var el = new HeatFElement(id);
                    el.nodes.Add(nodes[i][j]);
                    el.nodes.Add(nodes[i + 1][j]);
                    el.nodes.Add(nodes[i + 1][j + 1]);
                    el.nodes.Add(nodes[i][j + 1]);
                    //
                    els.Add(el);
                    id++;
                }
            }

            return els;
        }

        /// <summary>
        /// Prints node as three ordered number (Id,X1,x2)
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="tw"> File stream to export node coordinates</param>
        public static void printNodes(ThermoNode[][] nodes, StreamWriter tw)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                for (int j = 0; j < nodes[0].Length; j++)
                {
                    tw.WriteLine($"{nodes[i][j].id},{nodes[i][j].x1},{nodes[i][j].x2}");
                }
            }
        }
    }
}
