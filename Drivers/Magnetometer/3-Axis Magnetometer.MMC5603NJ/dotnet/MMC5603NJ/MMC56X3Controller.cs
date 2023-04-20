using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;

namespace MMC5603NJ {
    public class MMC56X3Controller {
        DUEController dueController;
        public byte SlaveAddress {
            get;
        }


        private int x;
        private int y;
        private int z;

        private int odr_cache = 0;
        private int ctrl2_cache = 0;

        public MMC56X3Controller(DUEController dueController, byte slaveAddress = 0x30) {
            this.dueController = dueController;
            this.SlaveAddress = slaveAddress;

            var id = this.ReadRegister(MMC56X3_PRODUCT_ID);

            if (id != MMC56X3_CHIP_ID)
                throw new Exception("The device is not supported.");

            this.Reset();



        }


        private void Reset() {
            this.WriteRegister(MMC56X3_CTRL1_REG, 0x80);

            Thread.Sleep(20);

            this.odr_cache = 0;
            this.ctrl2_cache = 0;

            this.MagnetSetReset();
            this.SetContinuousMode(false);

        }

        private void MagnetSetReset() {

            this.WriteRegister(MMC56X3_CTRL0_REG, 0x80); // turn on set bit
            Thread.Sleep(1);
            this.WriteRegister(MMC56X3_CTRL0_REG, 0x10); // turn on reset bit
            Thread.Sleep(1);
        }

        public void SetContinuousMode(bool enable) {
            if (enable) {
                this.WriteRegister(MMC56X3_CTRL0_REG, 0x80); // turn on cmm_freq_en bit
                this.ctrl2_cache |= 0x10;    // turn on cmm_en bit
            }
            else {
                this.ctrl2_cache &= ~0x10; // turn off cmm_en bit
            }

            this.WriteRegister(MMC56X3_CTRL2_REG, (byte)this.ctrl2_cache);
        }

        public void SetDataRate(int value) {
            if (value < 0 || (value > 255 && value != 1000))
                throw new Exception("value must be in range [0,255] or 1000");

            if (value == 1000) {
                this.WriteRegister(MMC5603_ODR_REG, 255); // turn on set bit
                this.ctrl2_cache |= 0x80; //turn on hpower bit

                this.WriteRegister(MMC56X3_CTRL2_REG, (byte)this.ctrl2_cache);
            }

            else {
                this.WriteRegister(MMC5603_ODR_REG, (byte)value); // turn on set bit
                this.ctrl2_cache &= ~0x80; //turn on hpower bit

                this.WriteRegister(MMC56X3_CTRL2_REG, (byte)this.ctrl2_cache);
            }

        }

        public bool IsContinuousMode => (this.ctrl2_cache & 0x10) != 0;

        private bool CheckStatusBit(int bit, int shift) {
            var val = this.ReadRegister(MMC56X3_STATUS_REG, 4);

            val >>= shift;

            if ((val & ((1 << (bit)) - 1)) != 0)
                return true;

            return false;


        }

        public double Temperature {
            get {
                if (this.IsContinuousMode)
                    throw new Exception("Reading temperature does not support continuous mode");

                this.WriteRegister(MMC56X3_CTRL0_REG, 0x02); // TM_T trigger

                while (!this.CheckStatusBit(1, 7))
                    Thread.Sleep(5);

                var temp = this.ReadRegister(MMC56X3_OUT_TEMP) * 1.0;

                temp *= 0.8; //  0.8*C / LSB
                temp -= 70;  //  0 value is -75


                return temp;
            }
        }


        private void Update() {


            if (!this.IsContinuousMode) {
                this.WriteRegister(MMC56X3_CTRL0_REG, 0x01);

                while (!this.CheckStatusBit(1, 6))
                    Thread.Sleep(5);

            }

            var dataWrite = new byte[] { MMC56X3_OUT_X_L };
            var dataRead = new byte[9];

            this.dueController.I2c.WriteRead(this.SlaveAddress, dataWrite, 0, 1, dataRead, 0, 9);

            this.x = (int)((uint)(dataRead[0] << 12) | (uint)(dataRead[1] << 4) | (uint)(dataRead[6] >> 4));
            this.y = (int)((uint)(dataRead[2] << 12) | (uint)(dataRead[3] << 4) | (uint)(dataRead[7] >> 4));
            this.z = (int)((uint)(dataRead[4] << 12) | (uint)(dataRead[5] << 4) | (uint)(dataRead[8] >> 4));

            Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", dataRead[0], dataRead[1], dataRead[6], this.x));
            Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", (uint)(dataRead[0] << 12), (uint)(dataRead[1] << 4), (uint)(dataRead[6] >> 4), this.x));

            this.x = (int)(this.x - ((uint)1 << 19));
            this.y = (int)(this.y - ((uint)1 << 19));
            this.z = (int)(this.z - ((uint)1 << 19));


            //Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", dataRead[0], dataRead[1], dataRead[6], this.x));

        }

        public double X {
            get {
                this.Update();
               
                return this.x * 0.00625;
            }
        }
        public double Y {
            get {
                this.Update();
                return this.y * 0.00625;
            }
        }
        public double Z {
            get {
                this.Update();
                return this.z * 0.00625;
            }
        }


        private void WriteRegister(byte register, byte value) {
            var data = new byte[] { register, value };

            this.dueController.I2c.Write(this.SlaveAddress, data);
        }

        private uint ReadRegister(byte register, int count = 1) {
            var dataWrite = new byte[] { register };
            var dataRead = new byte[count];

            this.dueController.I2c.WriteRead(this.SlaveAddress, dataWrite, dataRead);

            if (count == 1)
                return dataRead[0];

            return (uint)((dataRead[0]) | (dataRead[1] << 8) | (dataRead[2] << 16) | (dataRead[3] << 24));

        }

        const byte MMC56X3_PRODUCT_ID = 0x39;
        const byte MMC56X3_CTRL0_REG = 0x1B;
        const byte MMC56X3_CTRL1_REG = 0x1C;
        const byte MMC56X3_CTRL2_REG = 0x1D;
        const byte MMC56X3_STATUS_REG = 0x18;
        const byte MMC56X3_OUT_TEMP = 0x09;
        const byte MMC56X3_OUT_X_L = 0x00;
        const byte MMC5603_ODR_REG = 0x1A;

        const byte MMC56X3_CHIP_ID = 0x10;
    }
}
