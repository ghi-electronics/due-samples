using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace Gyroscope {
    public class GyroscopeController {
        DUEController dueController;

        const byte SlaveAddress = 0x6A;
        const byte LSM6DS3_WHO_AM_I_REG = 0X0F;
        const byte LSM6DS3_CTRL1_XL = 0X10;
        const byte LSM6DS3_CTRL2_G = 0X11;

        const byte LSM6DS3_STATUS_REG = 0X1E;

        const byte LSM6DS3_CTRL7_G = 0X16;
        const byte LSM6DS3_CTRL8_XL = 0X17;

        const byte LSM6DS3_OUTX_L_G = 0X22;


        const byte LSM6DS3_OUTX_L_XL = 0X28;


        public float AccelerationSampleRate { get; } = 104.0F;
        public float GyroscopeSampleRate { get; } = 104.0F;
        public GyroscopeController(DUEController due) {
            this.dueController = due;

            this.Initialize();
        }

        private void Initialize() {
            var dataRead = new byte[1];
            var dataWrite = new byte[1];

            dataWrite[0] = LSM6DS3_WHO_AM_I_REG;// L3G4200D_WHO_AM_I;


            this.dueController.I2c.WriteRead(SlaveAddress, dataWrite, dataRead);

            //set the gyroscope control register to work at 104 Hz, 2000 dps and in bypass mode
            this.WriteRegister(LSM6DS3_CTRL2_G, 0x4C);

            // Set the Accelerometer control register to work at 104 Hz, 4 g,and in bypass mode and enable ODR/4
            // low pass filter (check figure9 of LSM6DS3's datasheet)
            this.WriteRegister(LSM6DS3_CTRL1_XL, 0x4A);

            // set gyroscope power mode to high performance and bandwidth to 16 MHz
            this.WriteRegister(LSM6DS3_CTRL7_G, 0x00);

            // Set the ODR config register to ODR/4
            this.WriteRegister(LSM6DS3_CTRL8_XL, 0x09);
        }


        private void WriteRegister(byte register, byte data) {
            var dataWrite = new byte[] { register, data };

            this.dueController.I2c.Write(SlaveAddress, dataWrite);

        }

        private byte ReadRegister(byte register) {
            var dataWrite = new byte[] { register };
            var dataRead = new byte[1];

            this.dueController.I2c.WriteRead(SlaveAddress, dataWrite, dataRead);

            return dataRead[0];
        }

        private byte[] ReadRegisters(byte register, int readcount) {
            var dataWrite = new byte[] { register };
            var dataRead = new byte[readcount];

            this.dueController.I2c.WriteRead(SlaveAddress, dataWrite, dataRead);

            return dataRead;
        }

        public bool ReadAcceleration(out float x, out float y, out float z) {
            x = 0;
            y = 0;
            z = 0;

            var data = this.ReadRegisters(LSM6DS3_OUTX_L_XL, 6);

            var raw0 = data[0] | (data[1] << 8);
            var raw1 = data[2] | (data[3] << 8);
            var raw2 = data[3] | (data[5] << 8);

            if (data != null) {
                x = (float)(raw0 * 4.0 / 32768.0);
                y = (float)(raw1 * 4.0 / 32768.0);
                z = (float)(raw2 * 4.0 / 32768.0);

                return true;
            }

            return false;
        }

        public bool AccelerationAvailable() {
            if ((this.ReadRegister(LSM6DS3_STATUS_REG) & 0x01) != 0) {
                return true;
            }

            return false;
        }

        //public bool GyroscopeAvailable() {
        //    if ((this.ReadRegister(LSM6DS3_STATUS_REG) & 0x02) != 0) {
        //        return true;
        //    }

        //    return false;
        //}

        //public bool ReadGyroscope(out float x, out float y, out float z) {
        //    x = 0;
        //    y = 0;
        //    z = 0;

        //    var data = this.ReadRegisters(LSM6DS3_OUTX_L_G, 6);

        //    var raw0 = data[0] | (data[1] << 8);
        //    var raw1 = data[2] | (data[3] << 8);
        //    var raw2 = data[3] | (data[5] << 8);


        //    if (data != null) {
        //        x = (float)(raw0 * 2000.0 / 32768.0);
        //        y = (float)(raw1 * 2000.0 / 32768.0);
        //        z = (float)(raw2 * 2000.0 / 32768.0);
        //        return true;
        //    }

        //    return false;

        //}
    }
}
