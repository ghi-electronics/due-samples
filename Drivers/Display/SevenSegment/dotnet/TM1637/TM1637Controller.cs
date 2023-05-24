using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace TM1637 {
    public class TM1637Controller {
        DUELinkController dueController;
        int pinClk;
        int pinDio;
        uint bitdelay;
        byte brightness;

        const byte MinusSegments = 0b01000000;
        const byte TM1637_I2C_COMM1 = 0x40;
        const byte TM1637_I2C_COMM2 = 0xC0;
        const byte TM1637_I2C_COMM3 = 0x80;
        static readonly byte[] digitToSegment = new byte[] {
        // XGFEDCBA
        0b00111111,    // 0
        0b00000110,    // 1
        0b01011011,    // 2
        0b01001111,    // 3
        0b01100110,    // 4
        0b01101101,    // 5
        0b01111101,    // 6
        0b00000111,    // 7
        0b01111111,    // 8
        0b01101111,    // 9
        0b01110111,    // A
        0b01111100,    // b
        0b00111001,    // C
        0b01011110,    // d
        0b01111001,    // E
        0b01110001     // F
        };

        const int INPUT = 0;
        const int OUTPUT = 1;

        public TM1637Controller(DUELinkController dueController, int pinClk, int pinDio) {
            this.dueController = dueController;
            this.pinClk = pinClk;
            this.pinDio = pinDio;
            this.bitdelay = 0;

            dueController.Digital.Read(this.pinClk, DUELinkController.Input.PULL_NONE);
            dueController.Digital.Read(this.pinDio, DUELinkController.Input.PULL_NONE);

            dueController.Digital.Write(this.pinClk, false);
            dueController.Digital.Write(this.pinDio, false);
        }

        public uint BitDelay {
            get => this.bitdelay;
            set => this.bitdelay = value;
        }
        private void Delay() {
            var expired = DateTime.Now.Ticks + this.bitdelay * 10;

            while (DateTime.Now.Ticks < expired) ;


        }

        private void Start() {
            this.PinMode(this.pinDio, OUTPUT);
            this.Delay();
        }

        private void Stop() {
            this.PinMode(this.pinDio, OUTPUT);
            this.Delay();
            this.PinMode(this.pinClk, INPUT);
            this.Delay();
            this.PinMode(this.pinDio, INPUT);
            this.Delay();
        }

        private bool WriteByte(byte b) {
            var data = b;

            // 8 Data Bits
            for (byte i = 0; i < 8; i++) {
                // CLK low
                this.PinMode(this.pinClk, OUTPUT);
                this.Delay();

                // Set data bit
                if ((data & 0x01) != 0)
                    this.PinMode(this.pinDio, INPUT);
                else
                    this.PinMode(this.pinDio, OUTPUT);

                this.Delay();

                // CLK high
                this.PinMode(this.pinClk, INPUT);
                this.Delay();
                data = (byte)(data >> 1);
            }
            // Wait for acknowledge
            // CLK to zero
            this.PinMode(this.pinClk, OUTPUT);
            this.PinMode(this.pinDio, INPUT);
            this.Delay();

            // CLK to high
            this.PinMode(this.pinClk, INPUT);
            this.Delay();
            var ack = this.dueController.Digital.Read(this.pinDio, DUELinkController.Input.PULL_NONE);
            if (ack == false)
                this.PinMode(this.pinDio, OUTPUT);


            this.Delay();
            this.PinMode(this.pinClk, OUTPUT);
            this.Delay();

            return ack;
        }

        private byte EncodeDigit(byte digit) => digitToSegment[digit & 0x0f];
        
        public void ShowNumberDec(int num, bool dot) => this.ShowNumberBase(10, num, dot);

        public void ShowNumberHex(int num, bool dot) => this.ShowNumberBase(16, num, dot);
        public void ShowNumberBase(int baseNum, int num, bool dot) {


            var digits = new byte[4];

            var newPos = 0;
            var posEnd = 0;
            var posStart = 0;
            var posLength  = num < 0 ? 3 : 4;

            var newNum = Math.Abs(num);

            for (var i = 0; i < posLength; i++) {
                newNum = (newNum / baseNum);

                if (newNum == 0) {
                    break;
                }

                posEnd++;
            }

            

            if (num < 0) {
                posEnd++;
                posStart++;
                digits[0] = MinusSegments;
            }

            newNum = Math.Abs(num);

            for (var i = posEnd; i >= posStart; i--) {

                var digit = newNum % baseNum;
                newNum =(newNum / baseNum);

                digits[i] = (byte)(this.EncodeDigit((byte)digit) );  
  

                if (newNum == 0) {
                    break;
                }
                else {

                }


            }

            if (dot) {
                digits[1] = (byte)(digits[1] | 0x80);
            }

            this.SetSegments(digits, (byte)4, (byte)newPos);
        }

        public void Clear() {
            var digits = new byte[4];

            this.SetSegments(digits, 4, 0);
        }
        private void PinMode(int pin, int mode) {
            if (mode == OUTPUT) {
                this.dueController.Digital.Write(pin, false);
            }
            else {
                this.dueController.Digital.Read(pin, DUELinkController.Input.PULL_NONE);
            }
        }

        public int Brightness {
            get => this.brightness & 0x07;
            set {
                if (value <   0 || value > 7) {
                    throw new ArgumentOutOfRangeException("brightness must be in range [0,7]");
                }

                if (value > 0) {
                    this.brightness = 8; // turn on
                    this.brightness = (byte)(this.brightness | (value & 0x7));
                }
                else
                    this.brightness = 0;
                
            }
        }
        public void SetBrightness(byte brightness, bool on) {
            if (brightness < 0 || brightness > 7) {
                throw new ArgumentOutOfRangeException("brightness must be in range [0,7]");
            }

            this.brightness = (byte)((brightness & 0x7) | (on ? 0x08 : 0x00));
        }

        public void SetSegments(byte[] segments, byte length, byte pos) {
            // Write COMM1
            this.Start();
            this.WriteByte(TM1637_I2C_COMM1);
            this.Stop();

            // Write COMM2 + first digit address
            this.Start();
            this.WriteByte((byte)(TM1637_I2C_COMM2 + (pos & 0x03)));

            // Write the data bytes
            for (var k = 0; k < length; k++)
                this.WriteByte(segments[k]);

            this.Stop();

            // Write COMM3 + brightness
            this.Start();
            this.WriteByte((byte)(TM1637_I2C_COMM3 + (this.brightness & 0x0f)));
            this.Stop();
        }

    }
}
