using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OpenTK;
using System.Globalization;
using System;

namespace OpenGL
{
    public struct Vertex
    {
        public Vertex(Vector3 position, Vector3 color)
        {
            this.position = position;
            this.color = color;
        }

        public Vector3 position;
        public Vector3 color;
    }
}
