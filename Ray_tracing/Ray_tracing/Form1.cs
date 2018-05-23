using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ray_tracing
{
    public partial class Form1 : Form
    {
        private Graphics gr;
        private RayTracing rt;

        public Form1()
        {
            InitializeComponent();
            gr = new Graphics();
            rt = new RayTracing();
        }

        private void glControl1_Paint(object sender, /*PaintEventArgs*/ EventArgs e)
        {
            //gr.Update();
            rt.Update();
            glControl1.SwapBuffers();
            //gr.DisableShaders();
            rt.DisableShaders();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                //glControl1_Paint(sender, e);
                rt.SetSize(glControl1.Width, glControl1.Height);
                glControl1.Invalidate();
            }
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1: rt.ChangeZ(0.1f); break;
                case Keys.D2: rt.ChangeZ(-0.1f); break;
                case Keys.A: rt.ChangeX(-0.1f); break;
                case Keys.D: rt.ChangeX(0.1f); break;
                case Keys.W: rt.ChangeY(0.1f); break;
                case Keys.S: rt.ChangeY(-0.1f); break;
                case Keys.J: rt.ChangeDirX(-0.1f); break;
                case Keys.L: rt.ChangeDirX(0.1f); break;
                case Keys.I: rt.ChangeDirY(0.1f); break;
                case Keys.K: rt.ChangeDirY(-0.1f); break;
                case Keys.D3: rt.ChangeDirZ(0.1f); break;
                case Keys.D4: rt.ChangeDirZ(-0.1f); break;
            }
            //label1.Text = Convert.ToString(rt.camdir);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }
    }
}
