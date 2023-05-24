using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace SmartHome {
    internal class WindowController {
        DUELinkController dueController;
        int pin;
        public WindowController(DUELinkController dueController, int pin) {
            this.dueController = dueController;
            this.pin = pin;
        }

        public void Open() => this.dueController.Servo.Set(this.pin, 180);
        public void Close() => this.dueController.Servo.Set(this.pin, 0);


    }
}
