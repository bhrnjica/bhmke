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
            var u = _2Dproblem1.Solve(Problem4);
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
                        Console.WriteLine("       \t\t {0}={1:G3}", displName, u[i, 3], decimal.Round((decimal)u[i, 3], 6));
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
            var e1 = new MKEFElement(1,MKEElementType.triangle, MKEPlaneType.stress);
            var e2 = new MKEFElement(2, MKEElementType.triangle, MKEPlaneType.stress);


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
            E = 3.0e+7;
            nu = 0.25;
            t = 0.036;
            var e = new List<MKEFElement>();
            var e1 = new MKEFElement(1, MKEElementType.triangle, MKEPlaneType.stress);
            var e2 = new MKEFElement(2, MKEElementType.triangle, MKEPlaneType.stress);

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
            n2.y = 160;


           // BC
            n2.v = 0;
            n2.u = 0;
            nodes.Add(n2);

            var n3 = new MKENode();
            n3.id = 3;
            n3.x = 120;
            n3.y = 160;

            //BC
            n3.fx = 800;
            n3.fy = 0;
            nodes.Add(n3);

            var n4 = new MKENode();
            n4.id = 4;
            n4.x = 120;
            n4.y = 0;
            //BC
            n4.fx = 800;
            n4.fy = 0;
            nodes.Add(n4);

            e1.nodes.Add(n1); e1.nodes.Add(n3); e1.nodes.Add(n2);
            e2.nodes.Add(n1); e2.nodes.Add(n4); e2.nodes.Add(n3);

            //
            e.Add(e1);
            e.Add(e2);

            return e;
        }

        //
        //https://onedrive.live.com/edit.aspx?cid=8b11bbbf3f0ed903&id=documents&resid=8B11BBBF3F0ED903!69817&app=OneNote&&wd=target%28%2F%2FSedmica%209.one%7C4eb47f0f-9cd2-4ee9-a70c-c3b5942f23d4%2FReddy%20Zadatak%7C074e3a36-a77d-4191-a1f9-1ceef1ccc8e9%2F%29
        //
        private static List<MKEFElement> Problem3(List<MKENode> nodes, ref double E, ref double nu, ref double t)
        {
            //material property
            E = 30.0e+6;//psi
            nu = 0.25;
            t = 0.036;//plate thickness
     double a = 120;// inches
     double b = 160;// inches
     double p0 = 10;//lb/in

            //
            var e = new List<MKEFElement>();
            var e1 = new MKEFElement(1, MKEElementType.triangle, MKEPlaneType.stress);
            var e2 = new MKEFElement(2, MKEElementType.triangle, MKEPlaneType.stress);


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
            n2.x = a;
            n2.y = 0;

            //Boundary condition
            n2.fx = p0*b/2.0;//BC
            n2.fy = 0;//BC
            nodes.Add(n2);

            var n3 = new MKENode();
            n3.id = 3;
            n3.x = 0;
            n3.y = b;

            //Boundary condition

            n3.u = 0;//BC
            n3.v = 0;//BC
            nodes.Add(n3);

            var n4 = new MKENode();
            n4.id = 4;
            n4.x = a;
            n4.y = b;
            //BC
            n4.fx = p0 * b / 2.0;//BC
            n4.fy = 0;//BC
            nodes.Add(n4);

            e1.nodes.Add(n1); e1.nodes.Add(n2); e1.nodes.Add(n4);
            e2.nodes.Add(n1); e2.nodes.Add(n4); e2.nodes.Add(n3);
            //
            e.Add(e1);
            e.Add(e2);
            return e;
        }

        private static List<MKEFElement> Problem4(List<MKENode> nodes, ref double E, ref double nu, ref double t)
        {
            //material property
            E = 2.0e+11;//
            nu = 0.3;
            t = 0.01;//m
           

            //
            var e = new List<MKEFElement>();
            var e1 = new MKEFElement(1, MKEElementType.triangle, MKEPlaneType.stress);
            var e2 = new MKEFElement(2, MKEElementType.triangle, MKEPlaneType.stress);
            var e3 = new MKEFElement(3, MKEElementType.triangle, MKEPlaneType.stress);
            var e4 = new MKEFElement(4, MKEElementType.triangle, MKEPlaneType.stress);


            var n1 = new MKENode();
            n1.id = 1;
            n1.x = 0;
            n1.y = 0;
            
            //Boundary condition
            n1.u = 0;//BC
            n1.v = 0;//BC

            nodes.Add(n1);

            var n2 = new MKENode();
            n2.id = 2;
            n2.x = 0.1;
            n2.y = 0;

            //Boundary condition
            n2.fx = 0;//BC
            n2.fy = 0;//BC
            nodes.Add(n2);

            var n3 = new MKENode();
            n3.id = 3;
            n3.x = 0.2;
            n3.y = 0;

            //Boundary condition
            n3.fx = 5000;//BC
            n3.fy = 0;//BC
            nodes.Add(n3);

            var n4 = new MKENode();
            n4.id = 4;
            n4.x = 0.2;
            n4.y = 0.1;
            //BC
            n4.fx = 5000;//BC
            n4.fy = 0;//BC
            nodes.Add(n4);

            var n5 = new MKENode();
            n5.id = 5;
            n5.x = 0.1;
            n5.y = 0.1;
            //BC
            n5.fx = 0;//BC
            n5.fy = 0;//BC
            nodes.Add(n5);

            var n6 = new MKENode();
            n6.id = 6;
            n6.x = 0;
            n6.y = 0.1;
            //BC
            n6.u = 0;//BC
            n6.v = 0;//BC
            nodes.Add(n6);

            //constructing finite elements
            e1.nodes.Add(n1); e1.nodes.Add(n2); e1.nodes.Add(n6);
            e2.nodes.Add(n2); e2.nodes.Add(n3); e2.nodes.Add(n5);
            e3.nodes.Add(n3); e3.nodes.Add(n4); e3.nodes.Add(n5);
            e4.nodes.Add(n5); e4.nodes.Add(n6); e4.nodes.Add(n2);

            //element collection
            e.Add(e1);
            e.Add(e2);
            e.Add(e3);
            e.Add(e4);
            return e;
        }



    }
}
