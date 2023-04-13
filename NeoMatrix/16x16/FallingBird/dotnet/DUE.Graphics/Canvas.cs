using DUE.Graphics;

namespace DUE.Graphics {
    public class Canvas {
        private readonly byte[] pixels;
        private readonly int width;
        private readonly int height;
        private readonly Func<int, int, int, int> pixelMapper;
        private readonly Action<byte[]> renderer;

        public Canvas(int width, int height, Func<int, int, int, int> pixelMapper, Action<byte[]> renderer) {
            this.pixels = new byte[width * height * 3];
            this.width = width;
            this.height = height;
            this.pixelMapper = pixelMapper;
            this.renderer = renderer;
        }

        public void SetPixel(int x, int y, Color color) {
            if (x < 0 || x >= this.width) return;
            if (y < 0 || y >= this.height) return;

            var offset = this.pixelMapper(x, y, this.width);
            this.pixels[offset + 0] = color.G;
            this.pixels[offset + 1] = color.R;
            this.pixels[offset + 2] = color.B;
        }

        public Color GetPixel(int x, int y) {
            if (x < 0 || x >= this.width) return Color.Black;
            if (y < 0 || y >= this.height) return Color.Black;

            var offset = this.pixelMapper(x, y, this.width);
            var g = this.pixels[offset + 0];
            var r = this.pixels[offset + 1];
            var b = this.pixels[offset + 2];
            return Color.FromRGB(r, g, b);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color) {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var sx = 1;
            var sy = 1;
            if (dx < 0) {
                dx = -dx;
                sx = -1;
            }

            if (dy < 0) {
                dy = -dy;
                sy = -1;
            }

            this.SetPixel(x1, y1, color);
            if (dx > dy) {
                var err = dx >> 1;
                while (x1 != x2 || y1 != y2) {
                    x1 += sx;
                    err -= dy;
                    if (err <= 0) {
                        err += dx;
                        if (y1 != y2) y1 += sy;
                    }
                    this.SetPixel(x1, y1, color);
                }
            }
            else {
                var err = dy >> 1;
                while (x1 != x2 || y1 != y2) {
                    y1 += sy;
                    err -= dx;
                    if (err <= 0) {
                        err += dy;
                        if (x1 != x2) x1 += sx;
                    }
                    this.SetPixel(x1, y1, color);
                }
            }
        }

        public void DrawCircle(int x, int y, int r, Color color) {
            if (r < 1) return;
            var x1 = -r;
            var y1 = 0;
            var err = 2 - 2 * r;
            while (x1 <= 0) {
                this.SetPixel(x - x1, y + y1, color);
                this.SetPixel(x - y1, y - x1, color);
                this.SetPixel(x + x1, y - y1, color);
                this.SetPixel(x + y1, y + x1, color);
                r = err;
                if (r > x1) {
                    x1++;
                    err += x1 * 2 + 1;
                }
                if (r <= y1) {
                    y1++;
                    err += y1 * 2 + 1;
                }
            }
        }

        public void DrawChar(byte[] font, char c, int x, int y, Color color) {
            var width = font[0];
            var height = font[1];

            for (var v = 0; v < height; v++) {
                int b = font[c * height + 2 + v];
                for(var h = 0; h < width; h++) {
                    if ((b & 0x80) != 0) {
                        this.SetPixel(x + h, y + v, color);
                    } else {
                        this.SetPixel(x + h, y + v, Color.Black);
                    }
                    b <<= 1;
                }
            }
        }

        public void DrawText(byte[] font, string text, int x, int y, Color color) {
            var width = font[0];
            var height = font[1];

            for (var i=0; i < text.Length; i++) {
                if (text[i] == '\n' || x + width > this.width) {
                    x = 0;
                    y += height;
                }
                if (text[i] != '\n') {
                    this.DrawChar(font, text[i], x, y, color);
                    x += width;
                }                
            }
        }

        public void Clear() => Array.Fill<byte>(this.pixels, 0);
        public void Render() => this.renderer(this.pixels);

        public int Width => this.width;
        public int Height => this.height;
    }
}
