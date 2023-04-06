using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace InkyBit {
    public class InkyBit {
        DUEController dueController;
        public InkyBit(DUEController due) {
            this.dueController = due;
            this.dueController.Digital.Write(CS, CS_INACTIVE);

            this.Clear();
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


        private const byte DRIVER_CONTROL = 0x01;
        private const byte GATE_VOLTAGE = 0x03;
        private const byte SOURCE_VOLTAGE = 0x04;
        private const byte DISPLAY_CONTROL = 0x07;
        private const byte NON_OVERLAP = 0x0B;
        private const byte BOOSTER_SOFT_START = 0x0C;
        private const byte GATE_SCAN_START = 0x0F;
        private const byte DEEP_SLEEP = 0x10;
        private const byte DATA_MODE = 0x11;
        private const byte SW_RESET = 0x12;
        private const byte TEMP_WRITE = 0x1A;
        private const byte TEMP_READ = 0x1B;
        private const byte TEMP_CONTROL = 0x1C;
        private const byte TEMP_LOAD = 0x1D;
        private const byte MASTER_ACTIVATE = 0x20;
        private const byte DISP_CTRL1 = 0x21;
        private const byte DISP_CTRL2 = 0x22;
        private const byte WRITE_RAM = 0x24;
        private const byte WRITE_ALTRAM = 0x26;
        private const byte READ_RAM = 0x25;
        private const byte VCOM_SENSE = 0x28;
        private const byte VCOM_DURATION = 0x29;
        private const byte WRITE_VCOM = 0x2C;
        private const byte READ_OTP = 0x2D;
        private const byte WRITE_LUT = 0x32;
        private const byte WRITE_DUMMY = 0x3A;
        private const byte WRITE_GATELINE = 0x3B;
        private const byte WRITE_BORDER = 0x3C;
        private const byte SET_RAMXPOS = 0x44;
        private const byte SET_RAMYPOS = 0x45;
        private const byte SET_RAMXCOUNT = 0x4E;
        private const byte SET_RAMYCOUNT = 0x4F;

        private const int DC = 12;// uBit.io.P12   // MICROBIT_PIN_P12
        private const int CS = 8;// uBit.io.P8    // MICROBIT_PIN_P8
        private const int RESET = 2;// uBit.io.P2 // MICROBIT_PIN_P2
        private const int BUSY = 16;// uBit.io.P16 // MICROBIT_PIN_P16


        const bool CS_ACTIVE = false;
        const bool CS_INACTIVE = true;
        const bool DC_DATA = true;
        const bool DC_COMMAND = false;

        private const int WIDTH = 250;
        private const int HEIGHT = 122;

        private const int COLS = 136;
        private const int ROWS = 250;
        private const int OFFSET_X = 0;
        private const int OFFSET_Y = 6;

        private byte[] buf_b = new byte[(COLS / 8) * ROWS];
        private byte[] buf_r = new byte[(COLS / 8) * ROWS];

        private byte[] luts = new byte[30]{
            0x02, 0x02, 0x01, 0x11, 0x12, 0x12, 0x22, 0x22, 0x66, 0x69,
            0x69, 0x59, 0x58, 0x99, 0x99, 0x88, 0x00, 0x00, 0x00, 0x00,
            0xF8, 0xB4, 0x13, 0x51, 0x35, 0x51, 0x51, 0x19, 0x01, 0x00
        };
        public void BusyWait() {
            while (this.dueController.Digital.Read(BUSY, DUEController.Input.PULL_UP))
            {
                Thread.Sleep(50);
            }
        }
        public void Clear() {
            Array.Clear(this.buf_r);
            Array.Fill<byte>(this.buf_b, 0xff);
        }

        public void SetPixel(int x, int y, int color) {
            if (x >= WIDTH) return;
            if (y >= HEIGHT) return;
            y += OFFSET_Y;
            y = COLS - 1 - y;
            var shift = 7 - (y % 8);
            y /= 8;
            var offset = (x * (COLS / 8)) + y;

            var byte_b = this.buf_b[offset] | (0b1 << shift);
            var byte_r = this.buf_r[offset] & ~(0b1 << shift);

            if (color == 2) {
                byte_r |= 0b1 << shift;
            }
            if (color == 1) {
                byte_b &= ~(0b1 << shift);
            }

            this.buf_b[offset] = (byte)byte_b;
            this.buf_r[offset] = (byte)byte_r;
        }

        public void SpiCommand(byte command, byte[] data, int len) {
            
            this.dueController.Digital.Write(CS, CS_ACTIVE);
            
            this.dueController.Digital.Write(DC, DC_COMMAND);
            var temp = new byte[1] { command };
            this.dueController.Spi.Write(temp);
            if (len > 0) {
             
                this.dueController.Digital.Write(DC, DC_DATA);

                for (var x = 0; x < len; x++) {
             
                    temp[0] = data[x];
                    this.dueController.Spi.Write(temp);
                }
            }            
            this.dueController.Digital.Write(CS, CS_INACTIVE);

        }

        void SpiCommand(byte command) => this.SpiCommand(command, null, 0);

        void SpiCommand(byte command, byte[] data) {
            this.dueController.Digital.Write(CS, CS_ACTIVE);
            this.dueController.Digital.Write(DC, DC_COMMAND);

            var temp = new byte[1] { command };
            this.dueController.Spi.Write(temp);
            this.dueController.Digital.Write(DC, DC_DATA);

            this.dueController.Spi.Write(data);            
            this.dueController.Digital.Write(CS, CS_INACTIVE);

        }

        void SpiData(byte[] data) {

            this.dueController.Digital.Write(CS, CS_ACTIVE);

            this.dueController.Digital.Write(DC, DC_DATA);
            this.dueController.Spi.Write(data);
            
            this.dueController.Digital.Write(CS, CS_ACTIVE);

        }
        public void Show() {            
            this.dueController.Digital.Write(RESET, false);
            
            Thread.Sleep(100);            
            this.dueController.Digital.Write(RESET, true);
            
            Thread.Sleep(100);

            this.SpiCommand(0x12);
            Thread.Sleep(500);
            this.BusyWait();

            this.SpiCommand(DRIVER_CONTROL, new byte[] { ROWS - 1, (ROWS - 1) >> 8, 0x00 });
            this.SpiCommand(WRITE_DUMMY, new byte[] { 0x1B });
            this.SpiCommand(WRITE_GATELINE, new byte[] { 0x0B });
            this.SpiCommand(DATA_MODE, new byte[] { 0x03 });
            this.SpiCommand(SET_RAMXPOS, new byte[] { 0x00, COLS / 8 - 1 });
            this.SpiCommand(SET_RAMYPOS, new byte[] { 0x00, 0x00, (ROWS - 1) & 0xFF, (ROWS - 1) >> 8 });
            this.SpiCommand(WRITE_VCOM, new byte[] { 0x70 });
            this.SpiCommand(WRITE_LUT, this.luts, this.luts.Length);
            this.SpiCommand(SET_RAMXCOUNT, new byte[] { 0x00 });
            this.SpiCommand(SET_RAMYCOUNT, new byte[] { 0x00, 0x00 });

            this.SpiCommand(WRITE_RAM);
            this.SpiData(this.buf_b);
            this.SpiCommand(WRITE_ALTRAM);
            this.SpiData(this.buf_r);

            this.BusyWait();
            this.SpiCommand(MASTER_ACTIVATE);
        }
    }
}
