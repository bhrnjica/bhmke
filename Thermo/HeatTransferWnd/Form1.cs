using HeatTransfer2D;
using mke_core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatTransferWnd
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float a = float.Parse(txt_a.Text);
            float b = float.Parse(txt_b.Text);
            int nv = int.Parse(txt_nv.Text);
            int nh = int.Parse(txt_nh.Text);
            float k= float.Parse(txt_k.Text);

            var a_parts = MeshGeneration.divideLength(a, nh, k);
            var b_parts = MeshGeneration.divideLength(b, nv, 1);
            ///node generation
            ThermoNode[][] nodes = MeshGeneration.GenerateNodes(a_parts, b_parts);
            //finite element generation
            List<HeatFElement> els = MeshGeneration.GenerateTElements(nodes);
            drawMesh(a,b, els);

        }

        private void drawMesh(float a, float b, List<HeatFElement> els)
        {
            float right = pictureBox1.Width;
            float top = pictureBox1.Height;
            var g = pictureBox1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            //g.DrawLine(Pens.Black, 0, 0, a, b);
            // g.ScaleTransform(a, b);
            foreach (var e in els)
                drawFiniteElement(g, e);
        }

        private void drawFiniteElement(Graphics g, FElementBase el)
        {
           // using (var pen = new Pen(Color.Black, 1f))
            {
                var pen = new Pen(Color.Black, 1f);
                g.DrawLine(Pens.Black, (float)el.nodes[0].x1, (float)el.nodes[0].x2, (float)el.nodes[1].x1, (float)el.nodes[1].x2);
                g.DrawLine(Pens.Black, (float)el.nodes[1].x1, (float)el.nodes[1].x2, (float)el.nodes[2].x1, (float)el.nodes[2].x2);
                g.DrawLine(Pens.Black, (float)el.nodes[2].x1, (float)el.nodes[2].x2, (float)el.nodes[00].x1, (float)el.nodes[0].x2);
               // g.DrawLine(Pens.Black, (float)el.nodes[3].x1, (float)el.nodes[3].x2, (float)el.nodes[0].x1, (float)el.nodes[0].x2);
                g.DrawString(el.id.ToString(), SystemFonts.DefaultFont, Brushes.Blue, (float)el.nodes[0].x1, (float)el.nodes[0].x2);
            }

           

        }
    }
}
