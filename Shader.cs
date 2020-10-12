using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenGL
{
    public class Shader
    {
        // Программа
        public int Handle;

        // Для scaleFactor
        Dictionary<string, int> uniforms = new Dictionary<string, int>();

        // Позиция атрибутов
        Dictionary<string, int> attribs = new Dictionary<string, int>();

        // Компилиция шейдеров, (шейдер - преобразование входа в выход)
        int CompileShader(string Path, ShaderType shaderType)
        {
            string shaderSource;

            // Считываем файл (исходник) с вершинами
            using (StreamReader reader = new StreamReader(Path, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }

            // Объявление шейдера
            var shader = GL.CreateShader(shaderType);
            // Передача исходников
            GL.ShaderSource(shader, shaderSource);
            // Компиляция
            GL.CompileShader(shader);

            // Для вывода ошибок компиляции, если есть
            var infoLog = GL.GetShaderInfoLog(shader);
            
            if (infoLog.Length != 0)
            {
                System.Console.WriteLine($"Косяк с шейдером: {Path}");
                System.Console.WriteLine(infoLog);
                return 0;
            }

            return shader;

        }

        // Программа = вершинный шейдер + программный шейдер
        // Конструктор
        public Shader(string vertexPath, string fragmentPath)
        {
            var vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
            var fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

            Handle = GL.CreateProgram();

            // Прикрепление шейдера к программе
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            // Соединение двух шейдеров - линковка
            GL.LinkProgram(Handle);

            // Освобождение шейдеров, после линковки программы, т к они больше не нужны
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);

            // Удаляем, так как шейдеры уже есть в Handle
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

        }

        public int GetUniformLocation(string name)
        {
            if (!uniforms.ContainsKey(name))
            {
                var location = GL.GetUniformLocation(Handle, name);
                
                if (location < 0)
                {
                    System.Console.WriteLine($"Нет параметров: {name}");
                    return -1;
                }

                uniforms.Add(name, location);
            }

            return uniforms[name];
        }

        public int GetAttributeLocation(string name)
        {
            if (!attribs.ContainsKey(name))
            {
                var location = GL.GetAttribLocation(Handle, name);
                if (location < 0)
                {
                    System.Console.WriteLine($"Нет атрибутов: {name}");
                    return -1;
                }

                attribs.Add(name, location);
            }

            return attribs[name];
        }

        public void SetUniform(string name, float val)
        {
            GL.Uniform1(GetUniformLocation(name), val);
        }

        public void SetUniform(string name, Matrix4 val)
        {
            GL.UniformMatrix4(GetUniformLocation(name), false, ref val);
        }

        // Функция для использования программы
        public void Use()
        {
            // Указываем, что хотим вызвать именно эту программу
            GL.UseProgram(Handle);
        }
    }
}
