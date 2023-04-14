using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace SmartHome {
    internal class SingleLedController {
        DUEController dueController;
        int pin;
        public SingleLedController(DUEController dueController, int pin) {
            this.pin = pin;
            this.dueController = dueController; 
        }

        public void TurnOn() => this.dueController.Digital.Write(this.pin, true);
        public void TurnOff() => this.dueController.Digital.Write(this.pin, false);

    }
}
