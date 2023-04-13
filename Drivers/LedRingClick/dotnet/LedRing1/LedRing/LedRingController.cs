using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace LedRing {
    public class LedRingController {
        DUEController dueController;

        private int resetPin;
        private int latchPin;
        private int dataPin;
        private int clockPin;

        private uint ledIndex;

        public LedRingController(DUEController due, int reset, int latch, int data, int clock) {
            this.dueController = due;
            this.clockPin = clock;
            this.resetPin = reset;
            this.latchPin = latch;
            this.dataPin = data;

            due.Digital.Write(this.resetPin, false);
            due.Digital.Write(this.resetPin, true);

            this.ledIndex = 0;

        }

        private void SoftSPI(byte b) {
            for (var i = 7; i >= 0; i--) {
                if ((b & (1 << i)) > 0)
                    this.dueController.Digital.Write(this.dataPin, true);
                else
                    this.dueController.Digital.Write(this.dataPin, false);
                this.Clock();
            }
        }

        public void Clear() => this.ledIndex = 0;
        public void Show() {
            this.SoftSPI((byte)(this.ledIndex >> 0));
            this.SoftSPI((byte)(this.ledIndex >> 8));
            this.SoftSPI((byte)(this.ledIndex >> 16));
            this.SoftSPI((byte)(this.ledIndex >> 24));
            this.Latch();
        }
        public void Set(int id, bool on) {
            if (on) {
                this.ledIndex |= (uint)(1 << id);
            }
            else {
                this.ledIndex &= ~(uint)(1 << id);
            }
        }
        private void Latch() {
            this.dueController.Digital.Write(this.latchPin, true);
            this.dueController.Digital.Write(this.latchPin, false);
        }
        private void Clock() {
            this.dueController.Digital.Write(this.clockPin, true);
            this.dueController.Digital.Write(this.clockPin, false);
        }
    }
}
