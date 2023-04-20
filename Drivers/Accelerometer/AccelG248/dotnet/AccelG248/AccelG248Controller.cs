using GHIElectronics.DUE;

namespace Accelerometer {
    public class AccelG248Controller
    {
        DUEController dueController;

        private byte slaveAddres;
        public AccelG248Controller(DUEController due, byte slaveAddress = 0x1C)
        {
            this.dueController = due;
            this.slaveAddres = slaveAddress;

            this.WriteToRegister(0x2A, 1);
        }

        private void WriteToRegister(byte reg, byte value)
        {
            var writeData = new byte[2] { reg, value };

            this.dueController.I2c.Write(this.slaveAddres, writeData);

        }
        private byte[] ReadFromRegister(byte reg, int count)
        {
            var writeData = new byte[1] { reg };
            var readData = new byte[count];

            this.dueController.I2c.WriteRead(this.slaveAddres, writeData, readData);

            return readData;
        }

        public int X
        {
            get
            {
                var read = this.ReadFromRegister(0x1, 2);

                var v = (read[0] << 2) | ((read[1] >> 6) );
                if (v > 511)
                    v = v- 1024;

                return v;

            }
        }

        public int Y
        {
            get
            {
                var read = this.ReadFromRegister(0x3, 2);

                var v = (read[0] << 2) | ((read[1] >> 6) );
                if (v > 511)
                    v = v - 1024;

                return v;

            }
        }

        public int Z
        {
            get
            {
                var read = this.ReadFromRegister(0x5, 2);

                var v = (read[0] << 2) | ((read[1] >> 6) );
                if (v > 511)
                    v = v - 1024;

                return v;

            }
        }

    }
}
