using GHIElectronics.DUE;

namespace ST7735 {
    public enum ST7735CommandId : byte {
        //System
        NOP = 0x00,
        SWRESET = 0x01,
        RDDID = 0x04,
        RDDST = 0x09,
        RDDPM = 0x0A,
        RDDMADCTL = 0x0B,
        RDDCOLMOD = 0x0C,
        RDDIM = 0x0D,
        RDDSM = 0x0E,
        SLPIN = 0x10,
        SLPOUT = 0x11,
        PTLON = 0x12,
        NORON = 0x13,
        INVOFF = 0x20,
        INVON = 0x21,
        GAMSET = 0x26,
        DISPOFF = 0x28,
        DISPON = 0x29,
        CASET = 0x2A,
        RASET = 0x2B,
        RAMWR = 0x2C,
        RAMRD = 0x2E,
        PTLAR = 0x30,
        TEOFF = 0x34,
        TEON = 0x35,
        MADCTL = 0x36,
        IDMOFF = 0x38,
        IDMON = 0x39,
        COLMOD = 0x3A,
        RDID1 = 0xDA,
        RDID2 = 0xDB,
        RDID3 = 0xDC,

        //Panel
        FRMCTR1 = 0xB1,
        FRMCTR2 = 0xB2,
        FRMCTR3 = 0xB3,
        INVCTR = 0xB4,
        DISSET5 = 0xB6,
        PWCTR1 = 0xC0,
        PWCTR2 = 0xC1,
        PWCTR3 = 0xC2,
        PWCTR4 = 0xC3,
        PWCTR5 = 0xC4,
        VMCTR1 = 0xC5,
        VMOFCTR = 0xC7,
        WRID2 = 0xD1,
        WRID3 = 0xD2,
        NVCTR1 = 0xD9,
        NVCTR2 = 0xDE,
        NVCTR3 = 0xDF,
        GAMCTRP1 = 0xE0,
        GAMCTRN1 = 0xE1,
    }

    public enum DataFormat {
        Rgb565 = 0,
        Rgb444 = 1
    }
    public class ST7735Controller {
        private readonly byte[] buffer1 = new byte[1];
        private readonly byte[] buffer4 = new byte[4];

        private byte[] internalBuffer;

        private readonly int control;
        private readonly int reset;
        private readonly int cs;
        private readonly int backlight;        
        public int Width { get; private set; } = 160;
        public int Height { get; private set; } = 128;

        DUEController dueController;
        public ST7735Controller(DUEController bp, int chipselectPin, int controlPin, int resetPin, int backlightPin) {

            this.reset = resetPin;
            this.dueController = bp;
            this.control = controlPin;
            this.cs = chipselectPin;
            this.backlight = backlightPin;

            this.Reset();
            this.Initialize();

            this.SetDataFormat(DataFormat.Rgb565);
            this.SetDataAccessControl(true, false, true, false);
            this.SetDrawWindow(1, 2, this.Width - 1, this.Height - 1);

            this.Enable();

            this.internalBuffer = new byte[this.Width * this.Height * 2];
        }

        private void Reset() {
            this.dueController.Digital.Write(this.backlight, true);

            this.dueController.Digital.Write(this.reset, false);
            Thread.Sleep(50);

            this.dueController.Digital.Write(this.reset, true);
            Thread.Sleep(200);
        }

        private void Initialize() {
            this.SendCommand(ST7735CommandId.SWRESET);
            Thread.Sleep(120);

            this.SendCommand(ST7735CommandId.SLPOUT);
            Thread.Sleep(120);

            this.SendCommand(ST7735CommandId.FRMCTR1);
            this.SendData(0x01);
            this.SendData(0x2C);
            this.SendData(0x2D);

            this.SendCommand(ST7735CommandId.FRMCTR2);
            this.SendData(0x01);
            this.SendData(0x2C);
            this.SendData(0x2D);

            this.SendCommand(ST7735CommandId.FRMCTR3);
            this.SendData(0x01);
            this.SendData(0x2C);
            this.SendData(0x2D);
            this.SendData(0x01);
            this.SendData(0x2C);
            this.SendData(0x2D);

            this.SendCommand(ST7735CommandId.INVCTR);
            this.SendData(0x07);

            this.SendCommand(ST7735CommandId.PWCTR1);
            this.SendData(0xA2);
            this.SendData(0x02);
            this.SendData(0x84);

            this.SendCommand(ST7735CommandId.PWCTR2);
            this.SendData(0xC5);

            this.SendCommand(ST7735CommandId.PWCTR3);
            this.SendData(0x0A);
            this.SendData(0x00);

            this.SendCommand(ST7735CommandId.PWCTR4);
            this.SendData(0x8A);
            this.SendData(0x2A);

            this.SendCommand(ST7735CommandId.PWCTR5);
            this.SendData(0x8A);
            this.SendData(0xEE);

            this.SendCommand(ST7735CommandId.VMCTR1);
            this.SendData(0x0E);

            this.SendCommand(ST7735CommandId.GAMCTRP1);
            this.SendData(0x0F);
            this.SendData(0x1A);
            this.SendData(0x0F);
            this.SendData(0x18);
            this.SendData(0x2F);
            this.SendData(0x28);
            this.SendData(0x20);
            this.SendData(0x22);
            this.SendData(0x1F);
            this.SendData(0x1B);
            this.SendData(0x23);
            this.SendData(0x37);
            this.SendData(0x00);
            this.SendData(0x07);
            this.SendData(0x02);
            this.SendData(0x10);

            this.SendCommand(ST7735CommandId.GAMCTRN1);
            this.SendData(0x0F);
            this.SendData(0x1B);
            this.SendData(0x0F);
            this.SendData(0x17);
            this.SendData(0x33);
            this.SendData(0x2C);
            this.SendData(0x29);
            this.SendData(0x2E);
            this.SendData(0x30);
            this.SendData(0x30);
            this.SendData(0x39);
            this.SendData(0x3F);
            this.SendData(0x00);
            this.SendData(0x07);
            this.SendData(0x03);
            this.SendData(0x10);
        }
        public void Enable() => this.SendCommand(ST7735CommandId.DISPON);
        public void Disable() => this.SendCommand(ST7735CommandId.DISPOFF);

        private void SendCommand(ST7735CommandId command) {
            this.buffer1[0] = (byte)command;
            this.dueController.Digital.Write(this.control, false);
            this.dueController.Spi.Write(this.buffer1, this.cs);
        }

        private void SendData(byte data) {
            this.buffer1[0] = data;
            this.dueController.Digital.Write(this.control, true);
            this.dueController.Spi.Write(this.buffer1, this.cs);
        }

        private void SendData(byte[] data) {
            this.dueController.Digital.Write(this.control, true);
            this.dueController.Spi.Write(data, this.cs);
        }

        public void SetDataAccessControl(bool swapRowColumn, bool invertRow, bool invertColumn, bool useBgrPanel) {
            var val = default(byte);

            if (useBgrPanel) val |= 0b0000_1000;
            if (swapRowColumn) val |= 0b0010_0000;
            if (invertColumn) val |= 0b0100_0000;
            if (invertRow) val |= 0b1000_0000;

            this.SendCommand(ST7735CommandId.MADCTL);
            this.SendData(val);
        }

        private void SetDataFormat(DataFormat dataFormat) {
            switch (dataFormat) {
                case DataFormat.Rgb444:

                    this.SendCommand(ST7735CommandId.COLMOD);
                    this.SendData(0x03);

                    break;

                case DataFormat.Rgb565:

                    this.SendCommand(ST7735CommandId.COLMOD);
                    this.SendData(0x05);

                    break;

                default:
                    throw new NotSupportedException();
            }            
        }

        public void SetDrawWindow(int x, int y, int width, int height) {
            this.Width = width;
            this.Height = height;

            this.buffer4[1] = (byte)x;
            this.buffer4[3] = (byte)(x + width);
            this.SendCommand(ST7735CommandId.CASET);
            this.SendData(this.buffer4);

            this.buffer4[1] = (byte)y;
            this.buffer4[3] = (byte)(y + height);
            this.SendCommand(ST7735CommandId.RASET);
            this.SendData(this.buffer4);
        }

        private void SendDrawCommand() {
            this.SendCommand(ST7735CommandId.RAMWR);
            this.dueController.Digital.Write(this.control, true);
        }

        static void SwapEndianness(byte[] data) {

            for (var i = 0; i < data.Length; i += 2) {
                var t = data[i];
                data[i] = (byte)(data[i + 1]);
                data[i + 1] = t;

            }
        }

        public void Show() => this.Show(this.internalBuffer);
        public void Show(byte[] buffer, bool _4bpp = false) => this.Show(buffer, 0, (uint)buffer.Length, _4bpp);
        public void Show(byte[] buffer, uint offset, uint length, bool _4bpp = false) {
            if (buffer == null || (offset + length > buffer.Length))
                throw new ArgumentOutOfRangeException();

            this.SendDrawCommand();

            if (_4bpp) {
                this.dueController.Spi.Write4bpp(buffer, this.cs);

                return;
            }

            SwapEndianness(buffer);

            this.dueController.Spi.Write(buffer, this.cs);

            SwapEndianness(buffer);
        }
        public void Clear() => Array.Clear(this.internalBuffer);
        public void SetPixel(int x, int y, uint color) {
            if (x < 0 || y < 0 || x >= this.Width || y >= this.Height) return;

            var index = (y * this.Width + x) * 2;
            var clr = color;

            this.internalBuffer[index + 0] = (byte)(((clr & 0b0000_0000_0000_0000_0001_1100_0000_0000) >> 5) | ((clr & 0b0000_0000_0000_0000_0000_0000_1111_1000) >> 3));
            this.internalBuffer[index + 1] = (byte)(((clr & 0b0000_0000_1111_1000_0000_0000_0000_0000) >> 16) | ((clr & 0b0000_0000_0000_0000_1110_0000_0000_0000) >> 13));
        }

        public void DrawString(string text, uint color, int x, int y) => this.DrawString(text, color, x, y, 1, 1);
        public void DrawString(string text, uint color, int x, int y, int hScale, int vScale) {
            if (hScale == 0 || vScale == 0) throw new ArgumentNullException();
            var originalX = x;
            for (var i = 0; i < text.Length; i++) {
                if (text[i] >= 32) {
                    this.DrawCharacter(text[i], color, x, y, hScale, vScale);
                    x += (6 * hScale);
                }
                else {
                    if (text[i] == '\n') {
                        y += (9 * vScale);
                        x = originalX;
                    }
                    if (text[i] == '\r')
                        x = originalX;
                }
            }
        }
        public void DrawCharacter(char character, uint color, int x, int y) => this.DrawCharacter(character, color, x, y, 1, 1);

        public void DrawCharacter(char character, uint color, int x, int y, int hScale, int vScale) {
            var index = 5 * (character - 32);
            if (hScale != 1 || vScale != 1) {
                for (var horizontalFontSize = 0; horizontalFontSize < 5; horizontalFontSize++) {
                    for (var hs = 0; hs < hScale; hs++) {
                        for (var verticleFontSize = 0; verticleFontSize < 8; verticleFontSize++) {
                            for (var vs = 0; vs < vScale; vs++) {
                                if ((this.mono8x5[index + horizontalFontSize] & (1 << verticleFontSize)) != 0)
                                    this.SetPixel(x + (horizontalFontSize * hScale) + hs, y + (verticleFontSize * vScale) + vs, color);
                            }
                        }
                    }
                }
            }
            else {
                for (var horizontalFontSize = 0; horizontalFontSize < 5; horizontalFontSize++) {
                    var sx = x + horizontalFontSize;
                    var fontRow = this.mono8x5[index + horizontalFontSize];
                    for (var verticleFontSize = 0; verticleFontSize < 8; verticleFontSize++) {
                        if ((fontRow & (1 << verticleFontSize)) != 0) this.SetPixel(sx, y + verticleFontSize, color);
                    }
                }
            }
        }

        readonly byte[] mono8x5 = new byte[95 * 5] {
            0x00, 0x00, 0x00, 0x00, 0x00, /* Space	0x20 */
            0x00, 0x00, 0x4f, 0x00, 0x00, /* ! */
            0x00, 0x07, 0x00, 0x07, 0x00, /* " */
            0x14, 0x7f, 0x14, 0x7f, 0x14, /* # */
            0x24, 0x2a, 0x7f, 0x2a, 0x12, /* $ */
            0x23, 0x13, 0x08, 0x64, 0x62, /* % */
            0x36, 0x49, 0x55, 0x22, 0x20, /* & */
            0x00, 0x05, 0x03, 0x00, 0x00, /* ' */
            0x00, 0x1c, 0x22, 0x41, 0x00, /* ( */
            0x00, 0x41, 0x22, 0x1c, 0x00, /* ) */
            0x14, 0x08, 0x3e, 0x08, 0x14, /* // */
            0x08, 0x08, 0x3e, 0x08, 0x08, /* + */
            0x50, 0x30, 0x00, 0x00, 0x00, /* , */
            0x08, 0x08, 0x08, 0x08, 0x08, /* - */
            0x00, 0x60, 0x60, 0x00, 0x00, /* . */
            0x20, 0x10, 0x08, 0x04, 0x02, /* / */
            0x3e, 0x51, 0x49, 0x45, 0x3e, /* 0		0x30 */
            0x00, 0x42, 0x7f, 0x40, 0x00, /* 1 */
            0x42, 0x61, 0x51, 0x49, 0x46, /* 2 */
            0x21, 0x41, 0x45, 0x4b, 0x31, /* 3 */
            0x18, 0x14, 0x12, 0x7f, 0x10, /* 4 */
            0x27, 0x45, 0x45, 0x45, 0x39, /* 5 */
            0x3c, 0x4a, 0x49, 0x49, 0x30, /* 6 */
            0x01, 0x71, 0x09, 0x05, 0x03, /* 7 */
            0x36, 0x49, 0x49, 0x49, 0x36, /* 8 */
            0x06, 0x49, 0x49, 0x29, 0x1e, /* 9 */
            0x00, 0x36, 0x36, 0x00, 0x00, /* : */
            0x00, 0x56, 0x36, 0x00, 0x00, /* ; */
            0x08, 0x14, 0x22, 0x41, 0x00, /* < */
            0x14, 0x14, 0x14, 0x14, 0x14, /* = */
            0x00, 0x41, 0x22, 0x14, 0x08, /* > */
            0x02, 0x01, 0x51, 0x09, 0x06, /* ? */
            0x3e, 0x41, 0x5d, 0x55, 0x1e, /* @		0x40 */
            0x7e, 0x11, 0x11, 0x11, 0x7e, /* A */
            0x7f, 0x49, 0x49, 0x49, 0x36, /* B */
            0x3e, 0x41, 0x41, 0x41, 0x22, /* C */
            0x7f, 0x41, 0x41, 0x22, 0x1c, /* D */
            0x7f, 0x49, 0x49, 0x49, 0x41, /* E */
            0x7f, 0x09, 0x09, 0x09, 0x01, /* F */
            0x3e, 0x41, 0x49, 0x49, 0x7a, /* G */
            0x7f, 0x08, 0x08, 0x08, 0x7f, /* H */
            0x00, 0x41, 0x7f, 0x41, 0x00, /* I */
            0x20, 0x40, 0x41, 0x3f, 0x01, /* J */
            0x7f, 0x08, 0x14, 0x22, 0x41, /* K */
            0x7f, 0x40, 0x40, 0x40, 0x40, /* L */
            0x7f, 0x02, 0x0c, 0x02, 0x7f, /* M */
            0x7f, 0x04, 0x08, 0x10, 0x7f, /* N */
            0x3e, 0x41, 0x41, 0x41, 0x3e, /* O */
            0x7f, 0x09, 0x09, 0x09, 0x06, /* P		0x50 */
            0x3e, 0x41, 0x51, 0x21, 0x5e, /* Q */
            0x7f, 0x09, 0x19, 0x29, 0x46, /* R */
            0x26, 0x49, 0x49, 0x49, 0x32, /* S */
            0x01, 0x01, 0x7f, 0x01, 0x01, /* T */
            0x3f, 0x40, 0x40, 0x40, 0x3f, /* U */
            0x1f, 0x20, 0x40, 0x20, 0x1f, /* V */
            0x3f, 0x40, 0x38, 0x40, 0x3f, /* W */
            0x63, 0x14, 0x08, 0x14, 0x63, /* X */
            0x07, 0x08, 0x70, 0x08, 0x07, /* Y */
            0x61, 0x51, 0x49, 0x45, 0x43, /* Z */
            0x00, 0x7f, 0x41, 0x41, 0x00, /* [ */
            0x02, 0x04, 0x08, 0x10, 0x20, /* \ */
            0x00, 0x41, 0x41, 0x7f, 0x00, /* ] */
            0x04, 0x02, 0x01, 0x02, 0x04, /* ^ */
            0x40, 0x40, 0x40, 0x40, 0x40, /* _ */
            0x00, 0x00, 0x03, 0x05, 0x00, /* `		0x60 */
            0x20, 0x54, 0x54, 0x54, 0x78, /* a */
            0x7F, 0x44, 0x44, 0x44, 0x38, /* b */
            0x38, 0x44, 0x44, 0x44, 0x44, /* c */
            0x38, 0x44, 0x44, 0x44, 0x7f, /* d */
            0x38, 0x54, 0x54, 0x54, 0x18, /* e */
            0x04, 0x04, 0x7e, 0x05, 0x05, /* f */
            0x08, 0x54, 0x54, 0x54, 0x3c, /* g */
            0x7f, 0x08, 0x04, 0x04, 0x78, /* h */
            0x00, 0x44, 0x7d, 0x40, 0x00, /* i */
            0x20, 0x40, 0x44, 0x3d, 0x00, /* j */
            0x7f, 0x10, 0x28, 0x44, 0x00, /* k */
            0x00, 0x41, 0x7f, 0x40, 0x00, /* l */
            0x7c, 0x04, 0x7c, 0x04, 0x78, /* m */
            0x7c, 0x08, 0x04, 0x04, 0x78, /* n */
            0x38, 0x44, 0x44, 0x44, 0x38, /* o */
            0x7c, 0x14, 0x14, 0x14, 0x08, /* p		0x70 */
            0x08, 0x14, 0x14, 0x14, 0x7c, /* q */
            0x7c, 0x08, 0x04, 0x04, 0x08, /* r */
            0x48, 0x54, 0x54, 0x54, 0x24, /* s */
            0x04, 0x04, 0x3f, 0x44, 0x44, /* t */
            0x3c, 0x40, 0x40, 0x20, 0x7c, /* u */
            0x1c, 0x20, 0x40, 0x20, 0x1c, /* v */
            0x3c, 0x40, 0x30, 0x40, 0x3c, /* w */
            0x44, 0x28, 0x10, 0x28, 0x44, /* x */
            0x0c, 0x50, 0x50, 0x50, 0x3c, /* y */
            0x44, 0x64, 0x54, 0x4c, 0x44, /* z */
            0x08, 0x36, 0x41, 0x41, 0x00, /* { */
            0x00, 0x00, 0x77, 0x00, 0x00, /* | */
            0x00, 0x41, 0x41, 0x36, 0x08, /* } */
            0x08, 0x08, 0x2a, 0x1c, 0x08  /* ~ */
        };
    }
}