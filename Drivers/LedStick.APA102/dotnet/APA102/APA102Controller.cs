using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace APA102 {
    public class APA102Controller {
        DUEController dueController;
        public byte SlaveAddress { get; private set; }
        // Qwiic LED Stick commands
        const byte COMMAND_CHANGE_ADDRESS = 0xC7;
        const byte COMMAND_CHANGE_LED_LENGTH = 0x70;
        const byte COMMAND_WRITE_SINGLE_LED_COLOR = 0x71;
        const byte COMMAND_WRITE_ALL_LED_COLOR = 0x72;
        const byte COMMAND_WRITE_RED_ARRAY = 0x73;
        const byte COMMAND_WRITE_GREEN_ARRAY = 0x74;
        const byte COMMAND_WRITE_BLUE_ARRAY = 0x75;
        const byte COMMAND_WRITE_SINGLE_LED_BRIGHTNESS = 0x76;
        const byte COMMAND_WRITE_ALL_LED_BRIGHTNESS = 0x77;
        const byte COMMAND_WRITE_ALL_LED_OFF = 0x78;

        public int LedCount { get; private set; } = 10;
        public APA102Controller(DUEController dueController, byte slaveAddress = 0x23) {
            this.dueController = dueController;
            this.SlaveAddress = slaveAddress;
        }

        public void Set(int ledIndex, byte red, byte green, byte blue ) {

            if (ledIndex >= this.LedCount || ledIndex < 0)
                throw new Exception("ledIndex is out of range.");

            ledIndex++;
            var data = new byte[5] { COMMAND_WRITE_SINGLE_LED_COLOR, (byte)ledIndex, red, green, blue };

            this.dueController.I2c.Write(this.SlaveAddress, data);


        }

        public void Set(byte red, byte green, byte blue) {


            var data = new byte[4] { COMMAND_WRITE_ALL_LED_COLOR, red, green, blue };

            this.dueController.I2c.Write(this.SlaveAddress, data);


        }

        public void Brightness(uint ledIndex, uint brightness) {
            if (brightness > 31)
                brightness = 31;

            var data = new byte[2] { COMMAND_WRITE_SINGLE_LED_BRIGHTNESS, (byte)brightness };

            this.dueController.I2c.Write(this.SlaveAddress, data);
        }

        public void Clear() {
            var data = new byte[1] { COMMAND_WRITE_SINGLE_LED_BRIGHTNESS };

            this.dueController.I2c.Write(this.SlaveAddress, data);
        }

        public void ChangeAddress(byte newAddress) {
            if (newAddress < 0x08 || newAddress > 0x77)
                throw new Exception("new address must be in range [0x08, 0x77]");

            if (newAddress == this.SlaveAddress)
                return;

            var data = new byte[2] { COMMAND_CHANGE_ADDRESS, newAddress };

            this.dueController.I2c.Write(this.SlaveAddress, data);

            this.SlaveAddress = newAddress;

  
        }

        public void ChangeLedCount(int newCount) {
            if (newCount > 255) {
                throw new Exception("Support max 255 leds.");
            }
            var data = new byte[2] { COMMAND_CHANGE_LED_LENGTH, (byte)newCount };

            this.dueController.I2c.Write(this.SlaveAddress, data);

            this.LedCount = newCount;
        }
     


    }
}
