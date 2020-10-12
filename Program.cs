using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    class Program
    {

        public static void Main()
        {

            var application = new Application(800, 600, "OpenGL");
            application.Run(60);
        }
         
    }
}
