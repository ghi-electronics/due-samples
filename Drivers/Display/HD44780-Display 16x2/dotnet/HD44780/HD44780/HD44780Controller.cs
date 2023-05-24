using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace HD44780 {
    public class HD44780Controller {


        const uint RGB_ADDRESS = (0xc4 >> 1);
        const uint RGB_ADDRESS_V5 = (0x30);

        const uint REG_MODE1 = 0x00;
        const uint REG_MODE2 = 0x01;
        const uint REG_OUTPUT = 0x08;

        // commands
        const uint LCD_CLEARDISPLAY = 0x01;
        const uint LCD_RETURNHOME = 0x02;
        const uint LCD_ENTRYMODESET = 0x04;
        const uint LCD_DISPLAYCONTROL = 0x08;
        const uint LCD_CURSORSHIFT = 0x10;
        const uint LCD_FUNCTIONSET = 0x20;
        const uint LCD_SETCGRAMADDR = 0x40;
        const uint LCD_SETDDRAMADDR = 0x80;

        // flags for display entry mode
        const uint LCD_ENTRYRIGHT = 0x00;
        const uint LCD_ENTRYLEFT = 0x02;
        const uint LCD_ENTRYSHIFTINCREMENT = 0x01;
        const uint LCD_ENTRYSHIFTDECREMENT = 0x00;

        // flags for display on/off control
        const uint LCD_DISPLAYON = 0x04;
        const uint LCD_DISPLAYOFF = 0x00;
        const uint LCD_CURSORON = 0x02;
        const uint LCD_CURSOROFF = 0x00;
        const uint LCD_BLINKON = 0x01;
        const uint LCD_BLINKOFF = 0x00;

        // flags for display/cursor shift
        const uint LCD_DISPLAYMOVE = 0x08;
        const uint LCD_CURSORMOVE = 0x00;
        const uint LCD_MOVERIGHT = 0x04;
        const uint LCD_MOVELEFT = 0x00;

        // flags for function set
        const uint LCD_8BITMODE = 0x10;
        const uint LCD_4BITMODE = 0x00;
        const uint LCD_2LINE = 0x08;
        const uint LCD_1LINE = 0x00;
        const uint LCD_5x10DOTS = 0x04;
        const uint LCD_5x8DOTS = 0x00;

        DUELinkController dueController;
        private byte SlaveAddress { get; }

        private uint displayfunction = 0;
        private uint displaycontrol = 0;
        private uint displaymode = 0;



        public HD44780Controller(DUELinkController dueController, byte slaveAddress = 0x3E) {

            this.dueController = dueController;
            this.SlaveAddress = slaveAddress;

            this.displayfunction = LCD_DISPLAYON | LCD_2LINE;


            Thread.Sleep(50);

            this.WriteCommand((byte)(LCD_FUNCTIONSET | this.displayfunction));
            Thread.Sleep(5);

            this.WriteCommand((byte)(LCD_FUNCTIONSET | this.displayfunction));
            Thread.Sleep(5);

            this.WriteCommand((byte)(LCD_FUNCTIONSET | this.displayfunction));
            Thread.Sleep(5);

            this.WriteCommand((byte)(LCD_FUNCTIONSET | this.displayfunction));
            Thread.Sleep(5);

            this.displaycontrol = LCD_DISPLAYON | LCD_CURSOROFF | LCD_BLINKOFF;

            this.SetDisplay(true);

            this.Clear();

            this.displaymode = LCD_ENTRYLEFT | LCD_ENTRYSHIFTDECREMENT;
            this.WriteCommand((byte)(LCD_ENTRYMODESET | this.displaymode));

            if (this.SlaveAddress == RGB_ADDRESS_V5) {
                this.WriteRegister(0x00, 0x07); // reset the chip
                Thread.Sleep(1); // wait 200 us to complete

                this.WriteRegister(0x04, 0x15);// set all led always on
            }
            else {
                // backlight init
                this.WriteRegister((byte)REG_MODE1, 0);

                // set LEDs controllable by both PWM and GRPPWM registers
                this.WriteRegister((byte)REG_OUTPUT, 0);

                // set MODE2 values
                // 0010 0000 -> 0x20  (DMBLNK to 1, ie blinky mode)
                this.WriteRegister((byte)REG_MODE2, 0);

            }

        }


        private void WriteCommand(byte command) {
            var data = new byte[2];
            data[0] = 0x80;
            data[1] = command;

            this.dueController.I2c.Write(this.SlaveAddress, data);

        }

        private void WriteData(byte value) {
            var data = new byte[2];
            data[0] = 0x40;
            data[1] = value;
            this.dueController.I2c.Write(this.SlaveAddress, data);

        }

        public void DrawChar(char value) => this.WriteData((byte)value);

        public void DrawText(string text) {
            for (var i = 0; i < text.Length; i++) {
                this.DrawChar(text[i]);
            }
        }

        private void WriteRegister(byte register, byte value) {
            var data = new byte[2];
            data[0] = register;
            data[1] = value;

            this.dueController.I2c.Write(this.SlaveAddress, data);
        }

       

        public void Clear() {
            this.WriteCommand((byte)LCD_CLEARDISPLAY);
            Thread.Sleep(2);
        }
        public void Home() {
            this.WriteCommand((byte)LCD_RETURNHOME);
            Thread.Sleep(2);
        }

        public void SetCursor(uint col, uint row) {
            col = (row == 0 ? col | 0x80 : col | 0xc0);

            var data = new byte[] { 0x80, (byte)col };

            this.dueController.I2c.Write(this.SlaveAddress, data);
        }

        public void SetDisplay(bool on) {
            if (on) {
                this.displaycontrol |= LCD_DISPLAYON;

            }
            else {
                this.displaycontrol &= ~LCD_DISPLAYON;
            }

            this.WriteCommand((byte)(LCD_DISPLAYCONTROL | this.displaycontrol));

        }

        private bool cursorAvailable;
        public bool CursorAvailable {
            get => this.cursorAvailable;

            set {
                this.cursorAvailable = value;

                if (this.cursorAvailable) {
                    this.displaycontrol |= LCD_CURSORON;
                }
                else {
                    this.displaycontrol &= ~LCD_CURSORON;
                }

                this.WriteCommand((byte)(LCD_DISPLAYCONTROL | this.displaycontrol));
            }

        }

        private bool cursorBlink;
        public bool CursorBlink {
            get => this.cursorBlink;
            set {
                this.cursorBlink = value;
                if (this.cursorBlink) {
                    this.displaycontrol |= LCD_BLINKON;
                }
                else {
                    this.displaycontrol &= ~LCD_BLINKON;
                }

                this.WriteCommand((byte)(LCD_DISPLAYCONTROL | this.displaycontrol));
            }
        }

        public void CreateChar(uint location, byte[] charmap) {

            location &= 0x7;
            this.WriteCommand((byte)(LCD_SETCGRAMADDR | (location << 3)));

            var data = new byte[9];
            data[0] = 0x40;
            for (var i = 0; i < 8; i++) {
                data[i + 1] = charmap[i];
            }

            this.dueController.I2c.Write(this.SlaveAddress, data);

        }

        private bool leftToRight;
        public bool LeftToRight {
            get => this.leftToRight;
            set {
                this.leftToRight = value;
                if (this.leftToRight) {
                    this.displaymode |= LCD_ENTRYLEFT;
                }
                else {
                    this.displaymode &= ~LCD_ENTRYLEFT;
                }

                this.WriteCommand((byte)(LCD_ENTRYMODESET | this.displaymode));

            }

        }

        private bool autoScroll;
        public bool AutoScroll {
            get => this.autoScroll;

            set {
                this.autoScroll = value;
                if (this.autoScroll) {
                    this.displaymode |= LCD_ENTRYSHIFTINCREMENT;
                }
                else {
                    this.displaymode &= ~LCD_ENTRYSHIFTINCREMENT;
                }
                this.WriteCommand((byte)(LCD_ENTRYMODESET | this.displaymode));
            }
        }
    }



}
