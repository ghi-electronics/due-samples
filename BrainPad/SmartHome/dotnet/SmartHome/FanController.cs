using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace SmartHome {
    internal class FanController {
        DUEController dueController;
        int pinPlus;
        int pinMinus;
        public FanController(DUEController dueController, int pinPlus, int pinMinus) {



            this.dueController = dueController;
            this.pinPlus = pinPlus;
            this.pinMinus = pinMinus;

        }

        public void TurnOn(int speed, bool reverse) {

            if (speed > 100)
                throw new Exception("Max speed is 100%");

            var dutycyle = Scale(speed, 0, 100, 150, 500);


            if (reverse) {
                this.dueController.Digital.Write(this.pinPlus, false);                
                this.dueController.Analog.Write(this.pinMinus, dutycyle);
            }
            else {                ;
                this.dueController.Analog.Write(this.pinPlus, dutycyle);
                this.dueController.Digital.Write(this.pinMinus, false);
            }

        }

        public void TurnOff() {
            this.dueController.Digital.Write(this.pinPlus, false);
            this.dueController.Digital.Write(this.pinMinus, false);
        }

        internal static int Scale(double value, int originalMin, int originalMax, int scaleMin, int scaleMax) {
            var scale = (double)(scaleMax - scaleMin) / (originalMax - originalMin);
            var ret = (int)(scaleMin + ((value - originalMin) * scale));

            return ret > scaleMax ? scaleMax : (ret < scaleMin ? scaleMin : ret);
        }

    }
}
