using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace DUELink.Graphics {
    public struct Color {
        public readonly static Color Black = new Color(0x0000000);
        public readonly static Color Blue = new Color(0x000040);
        public readonly static Color Red = new Color(0x00400000);
        public readonly static Color Magenta = new Color(0x00400040);
        public readonly static Color Green = new Color(0x00004000);
        public readonly static Color Cyan = new Color(0x00004040);
        public readonly static Color Yellow = new Color(0x00404000);
        public readonly static Color White = new Color(0x00404040);
        public readonly static Color BrightBlue = new Color(0x000000ff);
        public readonly static Color BrightRed = new Color(0x00ff0000);
        public readonly static Color BrightGreen = new Color(0x0000ff00);
        public readonly static Color BrightCyan = new Color(0x0000ffff);
        public readonly static Color BrightYellow = new Color(0x00ffff00);
        public readonly static Color BrightWhite = new Color(0x00ffffff);

        public Color(uint color) => this.color = color;

        public static Color FromRGB(byte r, byte g, byte b) => new Color(((uint)r << 16) | ((uint)g << 8) | b);
        public static Color Random() => FromRGB((byte)System.Random.Shared.Next(255), (byte)System.Random.Shared.Next(255), (byte)System.Random.Shared.Next(255));

        public byte R => (byte)(this.color >> 16);
        public byte G => (byte)(this.color >> 8);
        public byte B => (byte)(this.color >> 0);

        public override bool Equals([NotNullWhen(true)] object? obj) {
            if (obj is not Color c) return false;
            return this.R == c.R && this.G == c.G && this.B == c.B;
        }

        public override int GetHashCode() => this.R.GetHashCode() * 31 ^ this.G.GetHashCode() * 31 ^ this.B.GetHashCode();

        public static bool operator ==(Color a, Color b) => a.Equals(b);
        public static bool operator !=(Color a, Color b) => !a.Equals(b);

        public static Color Lerp(Color from, Color to, double t) => Color.FromRGB((byte)(from.R + (to.R - from.R) * t), (byte)(from.G + (to.G - from.G) * t), (byte)(from.B + (to.B - from.B) * t));

        private readonly uint color;
    }
}
