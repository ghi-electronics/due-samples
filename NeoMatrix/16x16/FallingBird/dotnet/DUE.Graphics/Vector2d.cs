using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUELink.Graphics {
    public struct Vector2d {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2d(double x, double y) {
            this.X = x;
            this.Y = y;
        }

        public static Vector2d RandomDirection(double scale) {
            var angle = Random.Shared.NextDouble() * (2 * Math.PI);
            return new Vector2d(scale * Math.Cos(angle), scale * Math.Sin(angle));
        }

        public static Vector2d operator +(Vector2d a, Vector2d b) => new Vector2d(a.X + b.X, a.Y + b.Y);
        public static Vector2d operator -(Vector2d a, Vector2d b) => new Vector2d(a.X - b.X, a.Y - b.Y);
        public static Vector2d operator *(Vector2d a, double scale) => new Vector2d(a.X * scale, a.Y * scale);
    }
}
