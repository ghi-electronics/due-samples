using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteMaster {
    internal class Sprite {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }

        public int Width { get; set; }

        public int[] Image { get; set; }
        public char Character { get; set; }


        public Sprite() { }


        public Sprite(string name, int x, int y, int height, int width, int[] image) {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;


        }
        public Sprite(string name, int x, int y, int height, int width) {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;

        }

        public int Center() {
            var center = this.Height / 2;

            return center;
        }
    }
}
