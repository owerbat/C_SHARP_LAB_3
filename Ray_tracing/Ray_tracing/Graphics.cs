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
    class Graphics
    {
        Vector3 cameraPosition = new Vector3(2, 3, 4);
        Vector3 cameraDirection = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        string glVersion;
        string glslVersion;

        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        int vaoHandle;

        public Graphics()
        {
            //glVersion = GL.GetString(StringName.Version);
            //glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
        }

        public void Draw()
        {
            InitShaders();

            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //glControl1.SwapBuffers();
            //GL.UseProgram(0);
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
            address = GL.CreateShader(type);
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


            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };

            int[] vboHandlers = new int[2];
            GL.GenBuffers(2, vboHandlers);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);

            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * positionData.Length),
                          positionData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                         (IntPtr)(sizeof(float) * colorData.Length),
                         colorData, BufferUsageHint.StaticDraw);

            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void DisableShaders()
        {
            GL.DeleteProgram(BasicProgramID);
        }
    }
}
