using GHIElectronics.DUE;

namespace Accelerometer {
    public class AccelG248
    {
        DUEController dueController;

        const byte SlaveAddress = 0x1C;
        public AccelG248(DUEController due)
        {
            this.dueController = due;

            this.WriteToRegister(0x2A, 1);
        }

        private void WriteToRegister(byte reg, byte value)
        {
            var writeData = new byte[2] { reg, value };

            this.dueController.I2c.Write(SlaveAddress, writeData);

        }
        private byte[] ReadFromRegister(byte reg, int count)
        {
            var writeData = new byte[1] { reg };
            var readData = new byte[count];

            this.dueController.I2c.WriteRead(SlaveAddress, writeData, readData);

            return readData;
        }

        public int X
        {
            get
            {
                var read = this.ReadFromRegister(0x1, 2);

                var v = (read[0] << 2) | ((read[1] >> 6) & 0x3F);
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

                var v = (read[0] << 2) | ((read[1] >> 6) & 0x3F);
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

                var v = (read[0] << 2) | ((read[1] >> 6) & 0x3F);
                if (v > 511)
                    v = v - 1024;

                return v;

            }
        }

    }
}
