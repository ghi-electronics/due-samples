using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace LedBar {
    public class LedBarController {
        DUELinkController dueController;

        int pinClock;
        int pinData;

        public int LedNum { get; } = 10;
        public bool ReverseShow { get; set; } = false;

        byte[] leds;

        const int INPUT = 0;
        const int OUTPUT = 1;


        public LedBarController(DUELinkController dueController, int pinClock, int pinData) {
            this.dueController = dueController;
            this.pinClock = pinClock;
            this.pinData = pinData;
            
            this.leds = new byte[this.LedNum];

            Array.Clear(this.leds);

            this.PinMode(this.pinClock, OUTPUT);
            this.PinMode(this.pinData, OUTPUT);


        }

        private void PinMode(int pin, int mode) {
            if (mode == OUTPUT) {
                this.dueController.Digital.Write((int)pin, false);
            }
            else if (mode == INPUT) {
                this.dueController.Digital.Read((int)pin, DUELinkController.Input.PULL_NONE);
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

                this.Send(0x00); //send cmd(0x00)

                for (var i = this.LedNum ; i> 0;i--) {
                    this.Send(this.leds[i-1]);
                }
                for (uint i = 0; i < 12 - this.LedNum; i++) {
                    this.Send(0x00);
                }
            }
            else {
                this.Send(0x00); //send cmd(0x00)

                for (var i = 0; i < this.LedNum; i++) {
                    this.Send(this.leds[i]);
                }

                for (uint i = 0; i < 12 - this.LedNum; i++) {
                    this.Send(0x00);
                }
            }

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

        public void SetLed(int ledIdx, int brightness) {
            ledIdx = ledIdx > this.LedNum ? this.LedNum : ledIdx;

            brightness = brightness > 255 ? 255 : brightness;

            this.leds[ledIdx] = (byte)brightness;
        }

        public void Show() {
            this.Send();
            this.Latch();
        }

        public void Clear() => Array.Clear(this.leds);


    }
}
