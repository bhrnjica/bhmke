using MKELib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKEConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MKE2D _2Dproblem1 = new MKE2D();
            var u = _2Dproblem1.Solve(Problem2);
            printDisplacements(u);


            Console.Read();
        }

        private static void printDisplacements(Matrix u)
        {
            Console.WriteLine("----- DISPLACEMENT RESULTS------");
            for (int i = 0; i < u.rows; i++)
            {
                int type = (int)u[i, 1];
                if (type == 1)
                {
                    var displName = i % 2 == 1 ? "v" : "u";
                    var val = Decimal.Parse(u[i, 3].ToString(), System.Globalization.NumberStyles.Any);

                    if (displName == "u")
                        Console.WriteLine("NodeId:{0}:\t {1}={2:G3},", u[i, 0], displName, u[i, 3], decimal.Round((decimal)u[i, 3], 6));
                    else
                        Console.WriteLine("       \t\t {0}={1:G3} m.", displName, u[i, 3], decimal.Round((decimal)u[i, 3], 6));
                }

            }
            Console.WriteLine("----- END------");
        }

        private static List<MKEFElement> Problem1(List<MKENode> nodes, ref double E, ref double nu, ref double t)
        {
            //material property
            E = 2.0e+11;
            nu = 0.3;
            t = 0.01;

            var e = new List<MKEFElement>();
            var e1 = new MKEFElement(1);
            var e2 = new MKEFElement(2);


            var n1 = new MKENode();
            n1.id = 1;
            n1.x = 0;
            n1.y = 0;
            //BC
            n1.u = 0;
            n1.v = 0;
            nodes.Add(n1);

            var n2 = new MKENode();
            n2.id = 2;
            n2.x = 0;
            n2.y = 0.10;
            //BC
            n2.u = 0;
            n2.v = 0;
            nodes.Add(n2);

            var n3 = new MKENode();
            n3.id = 3;
            n3.x = 0.20;
            n3.y = 0.10;
            //BC
            n3.fx = 5000;//BC
            n3.fy = 0;//BC
            nodes.Add(n3);

            var n4 = new MKENode();
            n4.id = 4;
            n4.x = 0.20;
            n4.y = 0;
            //BC
            n4.fx = 5000;//BC
            n4.fy = 0;//BC
            nodes.Add(n4);

            e1.nodes.Add(n1); e1.nodes.Add(n3); e1.nodes.Add(n2);
            e2.nodes.Add(n1); e2.nodes.Add(n4); e2.nodes.Add(n3);
            //
            e.Add(e1);
            e.Add(e2);
            return e;
        }
      
        private static List<MKEFElement> Problem2(List<MKENode> nodes, ref double E, ref double nu, ref double t)
        {
            //material property
            E = 2.0e+11;
            nu = 0.25;
            t = 0.015;
            var e = new List<MKEFElement>();
            var e1 = new MKEFElement(1);
            var e2 = new MKEFElement(2);

            var n1 = new MKENode();
            n1.id = 1;
            n1.x = 0;
            n1.y = 0;

            //BC
            n1.u = 0;
            n1.v = 0;


            nodes.Add(n1);

            var n2 = new MKENode();
            n2.id = 2;
            n2.x = 0.750;
            n2.y = 0;

            //BC at leaset one (displacement or load ) must be set to zero or boundary value.
           // n1.u = 0;
            n2.v = 0;
            n2.fx = 0;
            nodes.Add(n2);

            var n3 = new MKENode();
            n3.id = 3;
            n3.x = 0.750;
            n3.y = 0.500;

            //BC
            n3.fx = 50000;
            n3.fy = 0;
            nodes.Add(n3);

            var n4 = new MKENode();
            n4.id = 4;
            n4.x = 0;
            n4.y = 0.500;
            //BC
            n4.u = 0;
            n4.v = 0;
            nodes.Add(n4);

            e1.nodes.Add(n1); e1.nodes.Add(n3); e1.nodes.Add(n4);
            e2.nodes.Add(n1); e2.nodes.Add(n2); e2.nodes.Add(n3);

            //
            e.Add(e1);
            e.Add(e2);

            return e;
        }

    }
}
