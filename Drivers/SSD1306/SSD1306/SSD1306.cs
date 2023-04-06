using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace SSD1306 {
    public class SSD1306 {
        DUEController dueController;

        private readonly byte[] vram = new byte[128 * 64 / 8 + 1];
        private readonly byte[] buffer2 = new byte[2];

        const byte SlaveAddress = 0x3C;
        public int Width => 128;
        public int Height => 64;
        public SSD1306(DUEController due) {
            this.dueController = due;

            this.vram[0] = 0x40;

            this.SendCommand(0xAE); //turn off oled panel
            this.SendCommand(0x00); //set low column address
            this.SendCommand(0x10); //set high column address
            this.SendCommand(0x40); //set start line address
            this.SendCommand(0x81); //set contrast control register
            this.SendCommand(0xCF);
            this.SendCommand(0xA1); //set segment re-map 95 to 0
            this.SendCommand(0xA6); //set normal display
            this.SendCommand(0xA8); //set multiplex ratio(1 to 64)
            this.SendCommand(0x3F); //1/64 duty
            this.SendCommand(0xD3); //set display offset
            this.SendCommand(0x00); //not offset
            this.SendCommand(0xD5); //set display clock divide ratio/oscillator frequency
            this.SendCommand(0x80); //set divide ratio
            this.SendCommand(0xD9); //set pre-charge period
            this.SendCommand(0xF1);
            this.SendCommand(0xDA); //set com pins hardware configuration
            this.SendCommand(0x12);
            this.SendCommand(0xDB); //set vcomh
            this.SendCommand(0x40); //set startline 0x0
            this.SendCommand(0x8D); //set Charge Pump enable/disable
            this.SendCommand(0x14); //set(0x10) disable
            this.SendCommand(0xAF); //turn on oled panel
            this.SendCommand(0xC8); //mirror the screen

            // Mapping
            this.SendCommand(0x20);
            this.SendCommand(0x00);
            this.SendCommand(0x21);
            this.SendCommand(0);
            this.SendCommand(128 - 1);
            this.SendCommand(0x22);
            this.SendCommand(0);
            this.SendCommand(7);
        }

        private void SendCommand(byte cmd) {
            this.buffer2[1] = cmd;
            this.dueController.I2c.Write(SlaveAddress, this.buffer2);
        }

        public void SetColorFormat(bool invert) => this.SendCommand((byte)(invert ? 0xA7 : 0xA6));

        public void Show() => this.dueController.I2c.Write(SlaveAddress, this.vram);

        public void Show(byte[] buffer) => this.Show(buffer, 0, (uint)buffer.Length);
        public void Show(byte[] buffer, uint offset, uint length) {
            if (buffer == null || (offset + length > buffer.Length))
                throw new ArgumentOutOfRangeException();

            Array.Copy(buffer, offset, this.vram, 1, length);
            this.dueController.I2c.Write(SlaveAddress, this.vram);
        }
        public void SetPixel(int x, int y, uint color) {
            if (x < 0 || y < 0 || x >= this.Width || y >= this.Height) return;

            var index = (y / 8) * this.Width + x;

            if (color != 0) {
                this.vram[1 + index] |= (byte)(1 << (y % 8));
            }
            else {
                this.vram[1 + index] &= (byte)(~(1 << (y % 8)));
            }
        }

        public void Clear() => Array.Clear(this.vram, 1, this.vram.Length - 1);

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
