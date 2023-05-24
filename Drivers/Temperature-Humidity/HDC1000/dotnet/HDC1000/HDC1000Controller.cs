using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace HDC1000 {
    public class HDC1000Controller {
        DUELinkController dueController;
        private byte SlaveAddress { get; }

        const byte HDC1000_ADDR = 0x40;

        const byte HDC1000_TEMP = 0x00;
        const byte HDC1000_HUMI = 0x01;
        const byte HDC1000_CONFIG = 0x02;

        const byte HDC1000_SERID_1 = 0xFB;
        const byte HDC1000_SERID_2 = 0xFC;
        const byte HDC1000_SERID_3 = 0xFD;
        const byte HDC1000_MFID = 0xFE;
        const byte HDC1000_DEVID = 0xFF;

        const byte HDC1000_RST = 0x80;
        const byte HDC1000_HEAT_ON = 0x20;
        const byte HDC1000_HEAT_OFF = 0x00;
        const byte HDC1000_BOTH_TEMP_HUMI = 0x10;
        const byte HDC1000_SINGLE_MEASUR = 0x00;
        const byte HDC1000_TEMP_HUMI_14BIT = 0x00;
        const byte HDC1000_TEMP_11BIT = 0x40;
        const byte HDC1000_HUMI_11BIT = 0x01;
        const byte HDC1000_HUMI_8BIT = 0x02;

        public HDC1000Controller(DUELinkController dueController, byte slaveAddress = 0x40) {
            this.dueController = dueController;
            this.SlaveAddress = slaveAddress;




            var config = HDC1000_BOTH_TEMP_HUMI | HDC1000_TEMP_HUMI_14BIT | HDC1000_HEAT_ON | HDC1000_RST;
            this.SetConfig((byte)config);
        }

        private void WriteRegister(byte register) {
            var data = new byte[1];

            data[0] = register;

            this.dueController.I2c.Write(this.SlaveAddress, data);

            Thread.Sleep(20);

        }

        private void SetConfig(byte config) {
            var data = new byte[3];

            data[0] = HDC1000_CONFIG;
            data[1] = config;
            data[2] = 0;

            this.dueController.I2c.Write(this.SlaveAddress, data);
        }

        private ushort ReadRegister() {
            var data = new byte[2];
            this.dueController.I2c.Read(this.SlaveAddress, data);

            var value = data[0] << 8 | data[1];

            return (ushort)value;
        }

        public double Temperature  {
            get {
                this.WriteRegister(HDC1000_TEMP);

                var raw = this.ReadRegister() * 1.0;

                return raw / 65536.0 * 165.0 - 40.0;
            }
        }

        public double Humidity {
            get {
                this.WriteRegister(HDC1000_HUMI);

                var raw = this.ReadRegister() * 1.0;

                return raw / 65536.0 * 100.0;
            }
        }
    }
}
