using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace LM358 {
    public class LM358Controller {
        DUEController dueController;
        private int pin;

        public LM358Controller(DUEController dueController, int analogPin) {
            this.dueController = dueController;
            this.pin = analogPin;
        }

        public float Read() {
            var sensor_value = this.dueController.Analog.Read(this.pin);

            return sensor_value;
        }
    }
}
