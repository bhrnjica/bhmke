using System;
using System.Collections.Generic;
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
    }
}
