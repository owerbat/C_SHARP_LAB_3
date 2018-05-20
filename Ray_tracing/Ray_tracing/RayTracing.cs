using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ray_tracing
{
    class RayTracing
    {
        Vector3 campos = new Vector3(-0.3f, 0.0f, 1.0f);
        Vector3 cameraPosition = new Vector3(-0.3f, -0.0f, 1.0f);
        Vector3 cameraDirection = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 cameraUp = new Vector3(0.0f, 1.0f, 0.0f);

        string glVersion;
        string glslVersion;

        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        //Vector3[] vertdata = new Vector3[] { new Vector3(-1f, -1f, 0f), new Vector3(1f, -1f, 0f), new Vector3(1f, 1f, 0), new Vector3(-1f, 1f, 0f) };
        float[] vertdata = { -1f, -1f, 0.0f, -1f, 1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0f };

        int width;
        int height;
        int vertexbuffer;

        public void SetSize(int _width, int _height)
        {
            width = _width;
            height = _height;

            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)width / (float)height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);

            InitShaders();
        }

        public void Draw()
        {
            GL.UseProgram(BasicProgramID);
            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, "campos"), campos);
            GL.Uniform1(GL.GetUniformLocation(BasicProgramID, "aspect"), (float)width / (float)height);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            GL.UseProgram(0);
        }

        public void Render()
        {
            Draw();
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirection, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            Render();
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            glVersion = GL.GetString(StringName.Version);
            glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
            address = GL.CreateShader(type);
            if (address == 0)
            {
                throw new Exception("loadShader error");
            }
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void InitShaders()
        {
            // создание объекта программы
            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\basic.vs.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("..\\..\\basic.fs.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);
            //Компановка программы
            GL.LinkProgram(BasicProgramID);
            // Проверить успех компановки
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

            GL.GenBuffers(1, out vertexbuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * vertdata.Length), vertdata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void DisableShaders()
        {
            GL.DeleteProgram(BasicProgramID);
        }

        public void ChangeX(float step)
        {
            campos += new Vector3(step, 0, 0);
        }

        public void ChangeY(float step)
        {
            campos += new Vector3(0, step, 0);
        }

        public void ChangeZ(float step)
        {
            campos += new Vector3(0, 0, step);
        }
    }
}
