using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace SmartHome {
    internal class RgbLedController {
        DUELinkController dueController;
        public int LedMax { get; } = 4;

        public RgbLedController(DUELinkController dueController) => this.dueController = dueController;

        public void TurnOn(int ledIndex, byte red, byte green, byte blue) {
            if (ledIndex > 3 || ledIndex < 0)
                throw new Exception("Led id must be in range [0.3]");

            var color = (uint)(red << 16 | green << 8 | blue);

            this.dueController.Neo.SetColor(ledIndex, color);

            this.dueController.Neo.Show(1, this.LedMax);
        }
        public void TurnOff() {
            for (var i = 0; i < this.LedMax; i++) {
                this.dueController.Neo.SetColor(i, 0);
            }
            this.dueController.Neo.Show(1, this.LedMax);
        }

        public void TurnOn(byte red, byte green, byte blue) {
            for (var i = 0; i < this.LedMax; i++) {

                var color = (uint)(red << 16 | green << 8 | blue);

                this.dueController.Neo.SetColor(i, color);
            }
            this.dueController.Neo.Show(1, this.LedMax);
        }

     
    }
}
