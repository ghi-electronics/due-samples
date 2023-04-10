using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace SHT31 {
    public class SHT31Controller {

        const ushort SHT31_SOFT_RESET = 0x30A2;
        const ushort SHT31_HARD_RESET = 0x0006;

        const ushort SHT31_MEASUREMENT_FAST = 0x2416;
        const ushort SHT31_MEASUREMENT_SLOW = 0x2400;

        DUEController dueController;
        public byte SlaveAddress { get; }
        public bool FastMode { get; set; } = false;

        public SHT31Controller(DUEController due, byte slaveAddress) {
            this.SlaveAddress = slaveAddress;
            this.dueController = due;
            this.Reset(false);
        }

        private bool WriteCommand(ushort command) {
            var data = new byte[2];

            data[0] = (byte)(command >> 8);
            data[1] = (byte)(command & 0xFF);

            return this.dueController.I2c.Write(this.SlaveAddress, data);

        }

        private byte[] ReadBytes(int count) {

            var dataRead = new byte[count];

            if (this.dueController.I2c.Read(this.SlaveAddress, dataRead))
                return dataRead;

            return null;

        }
        public void Reset(bool hardreset) {
            this.WriteCommand(hardreset ? SHT31_HARD_RESET : SHT31_SOFT_RESET);

            Thread.Sleep(1);

        }
        private void Measurement() {
            this.WriteCommand(this.FastMode ? SHT31_MEASUREMENT_FAST : SHT31_MEASUREMENT_SLOW);

            Thread.Sleep(this.FastMode ? 4 : 15);
        }

        public float Temperature {
            get {
                var mA = -45;
                var mB = 175;
                var mC = 65535;

                this.Measurement();

                var buffer = this.ReadBytes(6);

                var t = (float)((buffer[0] << 8) + buffer[1]);

                return mA + mB * (t / mC);
            }
        }

        public float Humidity {
            get {
                var mX = 0;
                var mY = 100;
                var mZ = 65535;

                this.Measurement();

                var buffer = this.ReadBytes(6);

                var h = (float)((buffer[3] << 8) + buffer[4]);

                return mX + mY * (h / mZ);
            }
        }
    }
}
