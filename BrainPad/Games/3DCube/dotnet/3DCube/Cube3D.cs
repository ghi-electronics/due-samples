using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;
using static GHIElectronics.DUE.DUEController;

namespace _3DCube {
    public class Cube3D {
       
        DUEController dueController;
        public Cube3D(DUEController dueController) => this.dueController = dueController;
        static void Translate3Dto2D(Vector3[] points3D, Vector2[] points2D, Vector3 rotate, Vector3 position) {
            const int OFFSETX = 64;
            const int OFFSETY = 32;
            const int OFFSETZ = 50;

            var sinax = Math.Sin(rotate.X * Math.PI / 180);
            var cosax = Math.Cos(rotate.X * Math.PI / 180);
            var sinay = Math.Sin(rotate.Y * Math.PI / 180);
            var cosay = Math.Cos(rotate.Y * Math.PI / 180);
            var sinaz = Math.Sin(rotate.Z * Math.PI / 180);
            var cosaz = Math.Cos(rotate.Z * Math.PI / 180);

            for (var i = 0; i < 8; i++) {
                var x = points3D[i].X;
                var y = points3D[i].Y;
                var z = points3D[i].Z;

                var yt = y * cosax - z * sinax;  // rotate around the x axis
                var zt = y * sinax + z * cosax;  // using the Y and Z for the rotation
                y = yt;
                z = zt;

                var xt = x * cosay - z * sinay;  // rotate around the Y axis
                zt = x * sinay + z * cosay;         // using X and Z
                x = xt;
                z = zt;

                xt = x * cosaz - y * sinaz;         // finally rotate around the Z axis
                yt = x * sinaz + y * cosaz;         // using X and Y
                x = xt;
                y = yt;

                x = x + position.X;                 // add the object position offset
                y = y + position.Y;                 // for both x and y
                z = z + OFFSETZ - position.Z;       // as well as Z

                points2D[i].X = (x * 64 / z) + OFFSETX;
                points2D[i].Y = (y * 64 / z) + OFFSETY;
            }
        }

        public void Run() {
            // our object in 3D space
            var cube_points = new Vector3[8]
            {
                new Vector3(10,10,-10),
                new Vector3(-10,10,-10),
                new Vector3(-10,-10,-10),
                new Vector3(10,-10,-10),
                new Vector3(10,10,10),
                new Vector3(-10,10,10),
                new Vector3(-10,-10,10),
                new Vector3(10,-10,10),
            };

            // what we get back in 2D space!
            var cube2 = new Vector2[8]
            {
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
            };

            // the connections between the "dots"
            var start = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3 };
            var end = new int[12] { 1, 2, 3, 0, 5, 6, 7, 4, 4, 5, 6, 7 };

            var rot = new Vector3(0, 0, 0);
            var pos = new Vector3(0, 0, 0);
            

            var valx = 0;
            var valy = 0;
            var dir = 1;

            while (true) {
                double accelX = valx ;
                double accelY = valy;

             

                valx = valx + (dir * 10);
                valy = valy + (dir * 10);

                if (valx >= 360) {
                    dir = -1;
                }

                this.dueController.Display.Clear(0);
                rot.X = 360 - accelY;
                rot.Y = accelX;
                Translate3Dto2D(cube_points, cube2, rot, pos);

                for (var i = 0; i < start.Length; i++) {    // draw the lines that make up the object
                    var vertex = start[i];                  // temp = start vertex for this line
                    var sx = (int)cube2[vertex].X;          // set line start x to vertex[i] x position
                    var sy = (int)cube2[vertex].Y;          // set line start y to vertex[i] y position
                    vertex = end[i];                        // temp = end vertex for this line
                    var ex = (int)cube2[vertex].X;          // set line end x to vertex[i+1] x position
                    var ey = (int)cube2[vertex].Y;          // set line end y to vertex[i+1] y position
                    this.dueController.Display.DrawLine(1, sx, sy, ex, ey);
                }
                this.dueController.Display.DrawText("X: " + (accelX * 100).ToString("F0"), 1, 0, 55);
                this.dueController.Display.DrawText("Y: " + (accelY * 100).ToString("F0"), 1, 80, 55);
                this.dueController.Display.Show();
                Thread.Sleep(250);
            }
        }

        class Vector3 {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public Vector3(double x, double y, double z) {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
        }

        class Vector2 {
            public double X { get; set; }
            public double Y { get; set; }
            public Vector2(double x, double y) {
                this.X = x;
                this.Y = y;
            }
        }        
    }
}
