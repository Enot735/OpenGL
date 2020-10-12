using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.CompilerServices;

namespace OpenGL
{
    class Application : GameWindow
    {
        // конструктор для отрисовки окна
        public Application(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }

        private int VertexBufferObject; // VBO - чтобы передать вершины на видеокарту
        private int IndexBufferObject; // IBO - чтобы передать индексы, по которым дальше нужно будет отрисовать что-то
        private Shader shader; // Объявление переменной для шейдера
        private Mesh mesh; // Объявление переменной для меша
        private float angle = 0.0f; // Поворот

        protected override void OnLoad(EventArgs e)
        {
            // цвет для окна
            // сначала на заднем буфере, после на сам экран, иначе было бы мерцающее изображение
            // вызывается один раз, иначе будет логать
            // задает цвет, которым будем очищать наш буфер

            // red, green, blue, alpha
            GL.ClearColor(0.3f, 0.3f, 0.4f, 1.0f);
            // Подключение буфера глубины
            GL.Enable(EnableCap.DepthTest);

            shader = new Shader("Shader.v", "Shader.f");

            mesh = MeshLoader.LoadMesh("mesh/deer.obj");

            VertexBufferObject = GL.GenBuffer();

            // Вершины - ArrayBuffer, передается в шейдер
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length * Unsafe.SizeOf<Vertex>(), mesh.vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(shader.GetAttributeLocation("aPosition"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), 0);
            GL.EnableVertexAttribArray(shader.GetAttributeLocation("aPosition"));

            GL.VertexAttribPointer(shader.GetAttributeLocation("aColor"), 3, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), Unsafe.SizeOf<Vector3>());
            GL.EnableVertexAttribArray(shader.GetAttributeLocation("aColor"));

            // Индексы
            // В шейдер не передается, используется для отрисовки треугольников
            // Создание
            IndexBufferObject = GL.GenBuffer();
            // Привязка
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferObject);
            // Передача данных
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length * sizeof(uint), mesh.indices, BufferUsageHint.StaticDraw);

            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {

            // двойная буферизация
            // для отображения на основном экране
            // очистка заднего буфера
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Указываем, что используем именно этот шейдер
            shader.Use();

            // Рзмер
            shader.SetUniform("scaleFactor", 1);

            // Поворот
            var model = Matrix4.CreateRotationY(angle);
            shader.SetUniform("rotation", model);
            // Бесконечность не предел
            angle += 0.01f;

            // Функция отрисовки
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

    }
}
