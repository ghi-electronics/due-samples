using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace SmartHome {
    internal class WindowController {
        DUEController dueController;
        int pin;
        public WindowController(DUEController dueController, int pin) {
            this.dueController = dueController;
            this.pin = pin;
        }

        public void Open() => this.dueController.ServoMoto.Set(this.pin, 180);
        public void Close() => this.dueController.ServoMoto.Set(this.pin, 0);


    }
}
