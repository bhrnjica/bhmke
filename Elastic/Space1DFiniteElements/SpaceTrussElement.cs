using mke_core;
using System;

namespace Space1DFiniteElements
{
    class SpaceTrussElement
    {
        static void Main(string[] args)
        {
            /*
             Zadatak prostornog stapa: 
             * * 
             */

            //Podaci;
            double A = 2;// poprecni presjek stapa u cm
            double E = 2;//modul elasticnosti u  Gpa

            //modul elasticnosti u paskalima
            double ee = E*Math.Pow(10,9);


            //Globalni koordinatni sistem, koordinate cvorova u cm
            
            //cvor 1:
            double X1 = 20;
            double Y1 = 15;
            double Z1 = 45;

            //cvor 2:
            double X2 = -10;
            double Y2 = -15;
            double Z2 = -25;

            //Potrebno odrediti:
            //1. lokalnu matricu krutosti   -[k]
            //2. transformacijaku matricu   -[T]
            //3. globalnu matricu krutosti     -[K]

            //Rjesenje

            //1. Odredjivanje duzine konačnog elementa
            var le = Math.Sqrt((X2 - X1)* (X2 - X1) + (Y2 - Y1) * (Y2 - Y1) + (Z2 - Z1) * (Z2 - Z1));
            //ispis duzine konačnog elementa
            Console.WriteLine("Duzina konačnog elementa stapa le={0}",le);

            //2. Odredjivanje kosinusa ugla lokalne koordinate x' i osa X,Y i Z globalnog koordinatnog sistema
            var cx = (X2 - X1) / le;
            var cy = (Y2 - Y1) / le;
            var cz = (Z2 - Z1) / le;

            Console.WriteLine("Kosinusi uglova loklane ose x i osa G.K.S. cx={0}, cy={1} i cz={2}", cx, cy, cz);

            //3. lokalna matrica krutosti
            var pm = new Matrix(2, 2);
            pm[0, 0] = 1; pm[0, 1] = -1; pm[1, 0] = -1; pm[1, 1] = 1;
            Matrix lmk = (A * ee / le) * pm;
            //Lokalna mtrica krutosti
            Console.WriteLine("Lokalna matica krutosti");
            Console.WriteLine("lmk=");
            Console.WriteLine("{0}",  lmk.ToString());

            //4. transformacijska matrica
            Matrix T = new Matrix(2, 6);
            T[0, 0] = cx; T[0, 1] = cy; T[0, 2] = cz;
            T[1, 3] = cx; T[1, 4] = cy; T[1, 5] = cz;
            Console.WriteLine("Matrica transformacije");
            Console.WriteLine("[T]=");
            Console.WriteLine("{0}", T.ToString());

            //5. Globalna matrica krutoti
            //[K] = [T]^T [k] [T]
            Console.WriteLine("Globalna matica krutosti");
            Console.WriteLine("[K] = [T]^T [k] [T]=");
            var gmk = Matrix.Transpose(T) * lmk * T;
           
            Console.WriteLine("{0}", gmk.ToString());
        }
    }
}