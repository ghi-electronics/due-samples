using GHIElectronics.DUELink;
using System.Drawing;
using System.Runtime.InteropServices;

// Used to get the primary display screen resolution
[DllImport("user32.dll")]
static extern int GetSystemMetrics(SystemMetric smIndex);


var port = DUELinkController.GetConnectionPort();
var device = new DUELinkController(port);


var srcrect = new Rectangle(0, 0, GetSystemMetrics(SystemMetric.CXSCREEN), GetSystemMetrics(SystemMetric.CYSCREEN));
var srcbmp = new Bitmap(srcrect.Width, srcrect.Height);
var g = Graphics.FromImage(srcbmp);
var scaledbmp = new Bitmap(128, 64);
var gscaled = Graphics.FromImage(scaledbmp);

// Play with a few dithering options
var d = new AtkinsonDither(128, 64);
//var d = new FloydSteinbergDither(128, 64);
//var d = new StuckiDither(128, 64);
//var d = new ThresholdDither(128, 64);

while (true)
{
    g.CopyFromScreen(srcrect.Location, new Point(0, 0), srcrect.Size);

    gscaled.DrawImage(srcbmp,
        new Rectangle(0, 0, scaledbmp.Width, scaledbmp.Height),
        srcrect,
        GraphicsUnit.Pixel);

    d.Dither(scaledbmp);

    device.Display.DrawBuffer(d.Buffer);
}
    

abstract class Ditherer
{
    protected float[] _gray;
    protected uint[] _result;
    public int Width { get; set; }
    public int Height { get; set; }

    public Ditherer(int width, int height)
    {        
        Width = width;
        Height = height;
        _gray = new float[width * height];
        _result = new uint[width * height];
    }

    public virtual void Dither(Bitmap bmp)
    {
        // Prime gray scale image
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var c = bmp.GetPixel(x, y);
                _gray[y * Width + x] = c.GetBrightness();
            }
        }

        // Perform dithering
        Process();

        // Threshold image for bitonal display
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _result[y * Width + x] = (byte)(Math.Min(Math.Max(0, _gray[y * Width + x]),1) >= 0.5 ? 1 : 0);
            }
        }
    }

    public void SetColor(int x, int y, byte color)
    {
        _result[y * Width + x] = color;
    }

    public uint GetColor(int x, int y)
    {
        return _result[y * Width + x];
    }

    public uint[] Buffer { get { return _result; } }

    protected abstract void Process();

    protected void SetShade(int x, int y, float color)
    {
        if (x < 0 || y < 0) return;
        if (x >= Width || y >= Height) return;

        _gray[y * Width + x] = color;
    }

    protected float GetShade(int x, int y)
    {
        if (x < 0) x = 0;
        if (x >= Width) x = Width - 1;
        if (y < 0) y = 0;
        if (y >= Height) y = Height - 1;
        return _gray[y * Width + x];
    }

}

class FloydSteinbergDither : Ditherer
{
    public FloydSteinbergDither(int width, int height) : base(width, height)
    {
    }

    protected override void Process()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var op = GetShade(x, y);
                var np = (byte)(op >= 0.5 ? 1 : 0);
                SetShade(x, y, np);

                var err = op - np;
                var a = GetShade(x + 1, y) + err * 7.0f / 16.0f;
                var b = GetShade(x - 1, y + 1) + err * 3.0f / 16.0f;
                var c = GetShade(x, y + 1) + err * 5.0f / 16.0f;
                var d = GetShade(x + 1, y + 1) + err * 1.0f / 16.0f;
                SetShade(x + 1, y, a);
                SetShade(x - 1, y + 1, b);
                SetShade(x, y + 1, c);
                SetShade(x + 1, y + 1, d);
            }
        }
    }
}

class StuckiDither : Ditherer
{
    public StuckiDither(int width, int height) : base(width, height)
    {
    }

    protected override void Process()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var op = GetShade(x, y);
                var np = (byte)(op >= 0.5 ? 1 : 0);
                SetShade(x, y, np);

                var err = op - np;
                var a = GetShade(x + 1, y) + err * 8.0f / 42.0f;
                var b = GetShade(x + 2, y) + err * 4.0f / 42.0f;

                var c = GetShade(x - 2, y + 1) + err * 2.0f / 42.0f;
                var d = GetShade(x - 1, y + 1) + err * 4.0f / 42.0f;
                var e = GetShade(x, y + 1) + err * 8.0f / 42.0f;
                var f = GetShade(x + 1, y + 1) + err * 4.0f / 42.0f;
                var g = GetShade(x + 2, y + 1) + err * 2.0f / 42.0f;

                var h = GetShade(x - 2, y + 2) + err * 1.0f / 42.0f;
                var i = GetShade(x - 1, y + 2) + err * 2.0f / 42.0f;
                var j = GetShade(x, y + 2) + err * 4.0f / 42.0f;
                var k = GetShade(x + 1, y + 2) + err * 2.0f / 42.0f;
                var l = GetShade(x + 2, y + 2) + err * 1.0f / 42.0f;

                SetShade(x + 1, y, a);
                SetShade(x + 2, y, b);

                SetShade(x - 2, y + 1, c);
                SetShade(x - 1, y + 1, d);
                SetShade(x, y + 1, e);
                SetShade(x + 1, y + 1, f);
                SetShade(x + 2, y + 1, g);

                SetShade(x - 2, y + 2, h);
                SetShade(x - 1, y + 2, i);
                SetShade(x, y + 2, j);
                SetShade(x + 1, y + 2, k);
                SetShade(x + 2, y + 2, l);
            }
        }
    }
}

class AtkinsonDither : Ditherer
{
    private const float ErrFactor = 1f / 8f;
    public AtkinsonDither(int width, int height) : base(width, height)
    {
    }

    protected override void Process()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var op = GetShade(x, y);
                var np = (byte)(op >= 0.5 ? 1 : 0);
                SetShade(x, y, np);

                var err = op - np;
                var a = GetShade(x + 1, y) + err * ErrFactor;
                var b = GetShade(x + 2, y) + err * ErrFactor;

                var c = GetShade(x - 1, y + 1) + err * ErrFactor;
                var d = GetShade(x , y + 1) + err * ErrFactor;
                var e = GetShade(x + 1, y + 1) + err * ErrFactor;

                var f = GetShade(x, y + 2) + err * ErrFactor;

                SetShade(x + 1, y, a);
                SetShade(x + 2, y, b);

                SetShade(x - 1, y + 1, c);
                SetShade(x, y + 1, d);
                SetShade(x + 1, y + 1, e);

                SetShade(x, y + 2, f);
            }
        }
    }
}

class ThresholdDither : Ditherer
{
    private readonly float _brightnessThreshold;
    public ThresholdDither(int width, int height, float brightnessThreshold = 0.5f) : base(width, height)
    {
        _brightnessThreshold = brightnessThreshold;
    }

    public override void Dither(Bitmap bmp)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SetColor(x, y, (byte)(bmp.GetPixel(x, y).GetBrightness() >= _brightnessThreshold ? 1 : 0));
            }
        }
    }

    protected override void Process()
    {
        throw new NotImplementedException();
    }
}

enum SystemMetric
{
    CXSCREEN = 0,
    CYSCREEN = 1,
    //... other constants not needed for this purpose
}
