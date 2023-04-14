using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace SmartHome {
    internal class RgbLedController {
        DUEController dueController;
        public int LedMax { get; } = 4;

        public RgbLedController(DUEController dueController) => this.dueController = dueController;

        public void TurnOn(int ledIndex, byte red, byte green, byte blue) {
            if (ledIndex > 3 || ledIndex < 0)
                throw new Exception("Led id must be in range [0.3]");

            this.dueController.Neo.SetColor(ledIndex, red, green, blue);

            this.dueController.Neo.Show(this.LedMax);
        }
        public void TurnOff() {
            for (var i = 0; i < this.LedMax; i++) {
                this.dueController.Neo.SetColor(i, 0, 0, 0);
            }
            this.dueController.Neo.Show(this.LedMax);
        }

        public void TurnOn(byte red, byte green, byte blue) {
            for (var i = 0; i < this.LedMax; i++) {
                this.dueController.Neo.SetColor(i, red, green, blue);
            }
            this.dueController.Neo.Show(this.LedMax);
        }

     
    }
}
