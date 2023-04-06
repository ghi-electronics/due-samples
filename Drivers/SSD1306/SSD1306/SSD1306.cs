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

        public void Flush() => this.dueController.I2c.Write(SlaveAddress, this.vram);

        public void Flush(byte[] buffer) => this.Flush(buffer, 0, (uint)buffer.Length);
        public void Flush(byte[] buffer, uint offset, uint length) {
            if (buffer == null || (offset + length > buffer.Length))
                throw new ArgumentOutOfRangeException();

            Array.Copy(buffer, offset, this.vram, 1, length);
            this.dueController.I2c.Write(SlaveAddress, this.vram);
        }
        public void SetPixel(int x, int y, bool color) {
            if (x < 0 || y < 0 || x >= this.Width || y >= this.Height) return;

            var index = (y / 8) * this.Width + x;

            if (color) {
                this.vram[1 + index] |= (byte)(1 << (y % 8));
            }
            else {
                this.vram[1 + index] &= (byte)(~(1 << (y % 8)));
            }
        }
    }
}
