using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUELink;

namespace BMP280 {
    public class BMP280Controller {
        DUELinkController dueController;
        public byte SlaveAddress { get; }

        // Calibratino data
        ushort dig_T1;
        short dig_T2;
        short dig_T3;
        ushort dig_P1;
        short dig_P2;
        short dig_P3;
        short dig_P4;
        short dig_P5;
        short dig_P6;
        short dig_P7;
        short dig_P8;
        short dig_P9;
        int fine;

        public BMP280Controller(DUELinkController due, byte slaveAddress = 0x77) {
            this.dueController = due;
            this.SlaveAddress= slaveAddress;

            var chip_id = this.ReadRegister8(BMP280_REG_CHIPID);

            if (chip_id != 0x58) {
                throw new Exception("The device is not supported.");
            }

            this.dig_T1 = this.ReadRegister16LE(BMP280_REG_DIG_T1);
            this.dig_T2 = this.ReadRegisterS16LE(BMP280_REG_DIG_T2);
            this.dig_T3 = this.ReadRegisterS16LE(BMP280_REG_DIG_T3);
            this.dig_P1 = this.ReadRegister16LE(BMP280_REG_DIG_P1);
            this.dig_P2 = this.ReadRegisterS16LE(BMP280_REG_DIG_P2);
            this.dig_P3 = this.ReadRegisterS16LE(BMP280_REG_DIG_P3);
            this.dig_P4 = this.ReadRegisterS16LE(BMP280_REG_DIG_P4);
            this.dig_P5 = this.ReadRegisterS16LE(BMP280_REG_DIG_P5);
            this.dig_P6 = this.ReadRegisterS16LE(BMP280_REG_DIG_P6);
            this.dig_P7 = this.ReadRegisterS16LE(BMP280_REG_DIG_P7);
            this.dig_P8 = this.ReadRegisterS16LE(BMP280_REG_DIG_P8);
            this.dig_P9 = this.ReadRegisterS16LE(BMP280_REG_DIG_P9);
            this.WriteRegister8(BMP280_REG_CONTROL, 0x3F);
        }

        public uint GetPressure() {
            long var1, var2, p;
            // Call getTemperature to get t_fine
            this.GetTemperature();

            var adc_P = this.ReadRegister24(BMP280_REG_PRESSUREDATA);
           

            adc_P >>= 4;
            var1 = ((long)this.fine) - 128000;


            var2 = var1 * var1 * (long)this.dig_P6;

            

            var2 = var2 + ((var1 * (long)this.dig_P5) << 17);

 
  
            var2 = var2 + (((long)this.dig_P4) << 35);

            var1 = ((var1 * var1 * (long)this.dig_P3) >> 8) + ((var1 * (long)this.dig_P2) << 12);
            var1 = (((((long)1) << 47) + var1)) * ((long)this.dig_P1) >> 33;
            if (var1 == 0) {
                return 0; // avoid exception caused by division by zero
            }

           
            p = 1048576 - adc_P;
            p = (((p << 31) - var2) * 3125) / var1;
            var1 = (((long)this.dig_P9) * (p >> 13) * (p >> 13)) >> 25;
            var2 = (((long)this.dig_P8) * p) >> 19;
            p = ((p + var1 + var2) >> 8) + (((long)this.dig_P7) << 4);
            return (uint)p / 256;
        }
        public double GetTemperature() {
            int var1, var2;
            var adc_T = this.ReadRegister24(BMP280_REG_TEMPDATA);

            adc_T >>= 4;
            var1 = (((adc_T >> 3) - ((int)(this.dig_T1 << 1))) *
                    ((int)this.dig_T2)) >> 11;
            var2 = (((((adc_T >> 4) - ((int)this.dig_T1)) *
                      ((adc_T >> 4) - ((int)this.dig_T1))) >> 12) *
                    ((int)this.dig_T3)) >> 14;
            this.fine = var1 + var2;
            var t = (double)((this.fine * 5 + 128) >> 8);
            return t / 100;
        }

        private double CalcAltitude(double p0, double p1, double t) {
            double c;
            c = (p0 / p1);
            c = Math.Pow(c, (1 / 5.25588)) - 1.0;
            c = (c * (t + 273.15)) / 0.0065;
            return c;
        }

        public double CalculateAltitude() {

            var t = this.GetTemperature();
            var p1 = this.GetPressure();
            return this.CalcAltitude(MSL, p1, t);
        }
        private byte ReadRegister8(byte register) {
            var data = this.ReadRegister(register, 1);
            return data[0];
        }

        private ushort ReadRegister16(byte register) {
            var data = this.ReadRegister(register, 2);

            return (ushort)(data[0] << 8 | data[1]);
        }


        private ushort ReadRegister16LE(byte reg) {
            var data = this.ReadRegister16(reg);
            return (ushort)((data >> 8) | (data << 8));
        }

        private short ReadRegisterS16(byte reg) => (short)this.ReadRegister16(reg);

        private short ReadRegisterS16LE(byte reg) => (short)this.ReadRegister16LE(reg);

        private int ReadRegister24(byte register) {
            var data = this.ReadRegister(register, 3);

            return (int)(data[0] << 16 | data[1] << 8 | data[2]);
        }

        private byte[] ReadRegister(byte register, int count) {

            var dataWrite = new byte[1] { register };
            var dataRead = new byte[count];

            this.dueController.I2c.WriteRead(this.SlaveAddress, dataWrite, dataRead);
            return dataRead;
        }

        private void WriteRegister(byte register, byte[] data) {
            var dataWrite = new byte[data.Length + 1];

            dataWrite[0] = register;

            Array.Copy(data, 0, dataWrite, 1, data.Length);

            this.dueController.I2c.Write(this.SlaveAddress, dataWrite);
        }

        const int MSL = 102009; // Mean Sea Level in Pa
        private void WriteRegister8(byte register, byte value) => this.WriteRegister(register, new byte[] { value });

        const byte BMP280_REG_DIG_T1 = 0x88;
        const byte BMP280_REG_DIG_T2 = 0x8A;
        const byte BMP280_REG_DIG_T3 = 0x8C;

        const byte BMP280_REG_DIG_P1 = 0x8E;
        const byte BMP280_REG_DIG_P2 = 0x90;
        const byte BMP280_REG_DIG_P3 = 0x92;
        const byte BMP280_REG_DIG_P4 = 0x94;
        const byte BMP280_REG_DIG_P5 = 0x96;
        const byte BMP280_REG_DIG_P6 = 0x98;
        const byte BMP280_REG_DIG_P7 = 0x9A;
        const byte BMP280_REG_DIG_P8 = 0x9C;
        const byte BMP280_REG_DIG_P9 = 0x9E;

        const byte BMP280_REG_CHIPID = 0xD0;
        const byte BMP280_REG_VERSION = 0xD1;
        const byte BMP280_REG_SOFTRESET = 0xE0;

        const byte BMP280_REG_CONTROL = 0xF4;
        const byte BMP280_REG_CONFIG = 0xF5;
        const byte BMP280_REG_PRESSUREDATA = 0xF7;
        const byte BMP280_REG_TEMPDATA = 0xFA;

    }
}
