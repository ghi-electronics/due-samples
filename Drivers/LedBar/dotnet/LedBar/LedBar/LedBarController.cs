using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace LedBar {
    public enum LedType {
        MaxLed10 = 10,
        MaxLed24 = 24,       
    }    
    public class LedBarController {
        DUEController dueController;

        uint pinClock;
        uint pinData;

        private LedType ledType;
        public uint LedNum { get; }
        public bool ReverseShow { get; set; }

        byte[] led;

        const int INPUT = 0;
        const int OUTPUT = 1;

        public LedBarController(DUEController dueController, uint pinClock, uint pinData, bool reverseShow, LedType ledtype) {
            this.dueController= dueController;
            this.pinClock= pinClock;
            this.pinData= pinData;
            this.ReverseShow= reverseShow;
            this.ledType = ledtype;


            var ledNum = (uint)ledtype;

            this.led = new byte[ledNum];

            for (uint i = 0; i < ledNum; i++) {
                this.led[i] = 0;
            }

            this.PinMode(this.pinClock, OUTPUT);
            this.PinMode(this.pinData, OUTPUT);

            this.LedNum = ledNum;
        }

        private void PinMode(uint pin, int mode) {
            if (mode == OUTPUT) {
                this.dueController.Digital.Write((int)pin, false);
            }
            else {
                this.dueController.Digital.Read((int)pin, DUEController.Input.PULL_NONE);
            }
        }

        private void Send(ushort bits) {
            var clk = false;
            for (var i = 0; i < 16; i++) {
                this.dueController.Digital.Write((int)this.pinData, (bits & 0x8000) != 0 ? true : false);
                this.dueController.Digital.Write((int)this.pinClock, clk);
                clk = !clk;
                bits <<= 1;
            }
        }

        private void Send() {
            if (this.ReverseShow) {
                if (this.ledType != LedType.MaxLed10) {
                    this.Send(0x00); //send cmd(0x00)

                    for (uint i = 24; i-- > 12;) {
                        this.Send(this.led[i]);
                    }

                    this.Send(0x00); //send cmd(0x00)

                    for (uint i = 12; i-- > 0;) {
                        this.Send(this.led[i]);
                    }
                }
                else {
                    this.Send(0x00); //send cmd(0x00)

                    for (var i = this.LedNum; i-- > 0;) {
                        this.Send(this.led[i]);
                    }
                    for (uint i = 0; i < 12 - this.LedNum; i++) {
                        this.Send(0x00);
                    }
                }

            }
            else {
                this.Send(0x00); //send cmd(0x00)

                for (var i = 0; i < 12; i++) {
                    this.Send(this.led[i]);
                }

                if (this.ledType == LedType.MaxLed10) {
                    this.Latch();
                    return;
                }

                this.Send(0x00); //send cmd(0x00)

                for (uint i = 12; i < 24; i++) {
                    this.Send(this.led[i]);
                }
            }
            this.Latch();
        }

        private void Latch() {
            this.dueController.Digital.Write((int)this.pinData, false);
            this.dueController.Digital.Write((int)this.pinClock, true); this.dueController.Digital.Write((int)this.pinClock, false);
            this.dueController.Digital.Write((int)this.pinClock, true); this.dueController.Digital.Write((int)this.pinClock, false);
            this.DelayMicroseconds(240);
            this.dueController.Digital.Write((int)this.pinData, true); this.dueController.Digital.Write((int)this.pinData, false);
            this.dueController.Digital.Write((int)this.pinData, true); this.dueController.Digital.Write((int)this.pinData, false);
            this.dueController.Digital.Write((int)this.pinData, true); this.dueController.Digital.Write((int)this.pinData, false);
            this.dueController.Digital.Write((int)this.pinData, true); this.dueController.Digital.Write((int)this.pinData, false);
            this.DelayMicroseconds(1);
            this.dueController.Digital.Write((int)this.pinClock, true);
            this.dueController.Digital.Write((int)this.pinClock, false);
        }

        private void DelayMicroseconds(uint us) {
            var expired = DateTime.Now.Ticks + us * 10;

            while (DateTime.Now.Ticks < expired) ;
        }

        public void SetLed(uint ledNo, int brightness) {
            ledNo = ledNo > this.LedNum ? this.LedNum : ledNo;

            brightness = brightness > 255 ? 255: brightness;
            
            this.led[ledNo] = (byte)brightness;
            this.Send();
        }

    }
}
