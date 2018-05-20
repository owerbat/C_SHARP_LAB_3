﻿using System;
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
        //private Graphics gr;
        private RayTracing rt;

        public Form1()
        {
            InitializeComponent();
            //gr = new Graphics();
            rt = new RayTracing();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            //gr.Update();
            rt.Update();
            glControl1.SwapBuffers();
            //gr.DisableShaders();
            rt.DisableShaders();
        }
    }
}