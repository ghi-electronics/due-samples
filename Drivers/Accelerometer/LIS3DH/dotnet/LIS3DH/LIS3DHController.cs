using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GHIElectronics.DUE;

namespace LIS3DH {
    public class LIS3DHController {
        DUEController dueController;

        public byte SlaveAddress { get; }

        private uint accRange;



        public LIS3DHController(DUEController dueController, byte slaveAddress = 0x19) {
            this.SlaveAddress = slaveAddress;
            this.dueController = dueController;


            var whoami = this.ReadRegister8(0xf);


            if (whoami != WHO_AM_I) {
                throw new Exception("The device is not supported");
            }

            var config5 = LIS3DHTR_REG_TEMP_ADC_PD_ENABLED |
                      LIS3DHTR_REG_TEMP_TEMP_EN_DISABLED;

            this.WriteRegister8(LIS3DHTR_REG_TEMP_CFG, (byte)config5);

            Thread.Sleep(LIS3DHTR_CONVERSIONDELAY);

            var config1 = LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_NORMAL | // Normal Mode
                      LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_ENABLE | // Acceleration Z-Axis Enabled
                      LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_ENABLE | // Acceleration Y-Axis Enabled
                      LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_ENABLE;

            this.WriteRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG1, (byte)config1);

            Thread.Sleep(LIS3DHTR_CONVERSIONDELAY);

            var config4 = LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_NOTUPDATED | // Continuous Update
                     LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_LSB |        // Data LSB @ lower address
                     LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_DISABLE |      // High Resolution Disable
                     LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_NORMAL |      // Normal Mode
                     LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_4WIRE;       // 4-Wire Interface

            this.WriteRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG4, (byte)config4);

            Thread.Sleep(LIS3DHTR_CONVERSIONDELAY);

            this.SetFullScaleRange(ScaleType.LIS3DHTR_RANGE_16G);
            //this.SetOutputDataRate(OdrType.LIS3DHTR_DATARATE_400HZ);

            this.SetOutputDataRate(OdrType.LIS3DHTR_DATARATE_50HZ);
            this.SetHighSolution(true);


        }

        public float X => this.GetAccelerationX();
        public float Y => this.GetAccelerationY();
        public float Z => this.GetAccelerationZ();

        //public double Temperature => this.GetTemperature();  
        private float GetAccelerationX() {
         
            var xAccelLo = this.ReadRegister8(LIS3DHTR_REG_ACCEL_OUT_X_L);
           
            var xAccelHi = this.ReadRegister8(LIS3DHTR_REG_ACCEL_OUT_X_H);
         
            var x = (ushort)((xAccelHi << 8) | xAccelLo);

            return (float)x / this.accRange;
        }

        private float GetAccelerationY() {
          
            var yAccelLo = this.ReadRegister8(LIS3DHTR_REG_ACCEL_OUT_Y_L);
           
            var yAccelHi = this.ReadRegister8(LIS3DHTR_REG_ACCEL_OUT_Y_H);
         
            var y = (ushort)((yAccelHi << 8) | yAccelLo);

            return (float)y / this.accRange;
        }

        private float GetAccelerationZ() {
         
            var zAccelLo = this.ReadRegister8(LIS3DHTR_REG_ACCEL_OUT_Z_L);
           
            var zAccelHi = this.ReadRegister8(LIS3DHTR_REG_ACCEL_OUT_Z_H);
         
            var z = (ushort)((zAccelHi << 8) | zAccelLo);
    
            return (float)z / this.accRange;
        }

        //public void EnableTemperature(bool enable) {

        //    var config5 = LIS3DHTR_REG_TEMP_ADC_PD_ENABLED |
        //              (enable ? LIS3DHTR_REG_TEMP_TEMP_EN_ENABLED : LIS3DHTR_REG_TEMP_TEMP_EN_DISABLED);

        //    this.WriteRegister8(LIS3DHTR_REG_TEMP_CFG, (byte)config5);
        //    Thread.Sleep(LIS3DHTR_CONVERSIONDELAY);

        //}
        public void SetHighSolution(bool enable) {
             
            var data = this.ReadRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG4);

            data = enable ? (byte)(data | LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_ENABLE) : (byte)(data & ~LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_ENABLE);

            this.WriteRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG4, data);
            return;
        }
        public void SetOutputDataRate(OdrType odr) {
            

            var data = this.ReadRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG1);

            data = (byte)(data & ~LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_MASK);
            data = (byte)(data | (byte)odr);

            this.WriteRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG1, data);
            Thread.Sleep(LIS3DHTR_CONVERSIONDELAY);
        }
        public void SetFullScaleRange(ScaleType range) {
            

            var data = (byte)this.ReadRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG4);

            data = (byte)(data & (~LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_MASK));
            data = (byte)(data | (byte)range);

            this.WriteRegister8(LIS3DHTR_REG_ACCEL_CTRL_REG4, data);

            Thread.Sleep(LIS3DHTR_CONVERSIONDELAY);

            switch ((byte)range) {
                case LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_16G:
                    this.accRange = 1280;
                    break;
                case LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_8G:
                    this.accRange = 3968;
                    break;
                case LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_4G:
                    this.accRange = 7282;
                    break;
                case LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_2G:
                    this.accRange = 16000;
                    break;
                default:
                    break;
            }
        }

        //private double GetTemperature() {
        //    var t = this.ReadRegister16(0x0c) * 1.0 / 256.0;
        //    t += 25;

        //    return t;
        //}

        private byte ReadRegister8(byte register) {
            var data = this.ReadRegister(register, 1);
            return data[0];
        }

        private ushort ReadRegister16(byte register) {
            var data = this.ReadRegister(register, 2);

            return (ushort)(data[0] | data[1] << 8);
        }
        private byte[] ReadRegister(byte register, int count) {

            if (count > 1) {
                register = (byte)(register | 0x80);
            }
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
        private void WriteRegister8(byte register, byte value) => this.WriteRegister(register, new byte[] { value });

        public enum PowerType // power mode
        {
            POWER_MODE_NORMAL = LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_NORMAL,
            POWER_MODE_LOW = LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_LOW
        };

        public enum ScaleType // measurement rage
        {
            LIS3DHTR_RANGE_2G = LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_2G,   //
            LIS3DHTR_RANGE_4G = LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_4G,   //
            LIS3DHTR_RANGE_8G = LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_8G,   //
            LIS3DHTR_RANGE_16G = LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_16G, //
        };

        public enum OdrType // output data rate
        {
            LIS3DHTR_DATARATE_POWERDOWN = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_PD,
            LIS3DHTR_DATARATE_1HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_1,
            LIS3DHTR_DATARATE_10HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_10,
            LIS3DHTR_DATARATE_25HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_25,
            LIS3DHTR_DATARATE_50HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_50,
            LIS3DHTR_DATARATE_100HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_100,
            LIS3DHTR_DATARATE_200HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_200,
            LIS3DHTR_DATARATE_400HZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_400,
            LIS3DHTR_DATARATE_1_6KH = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_1_6K,
            LIS3DHTR_DATARATE_5KHZ = LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_5K
        };


        const byte WHO_AM_I = 0b00110011;

        const int LIS3DHTR_CONVERSIONDELAY = 100;

        /**************************************************************************
            ACCELEROMETER REGISTERS
        **************************************************************************/
        const byte LIS3DHTR_REG_ACCEL_STATUS = (0x07); // Status Register
        const byte LIS3DHTR_REG_ACCEL_OUT_ADC1_L = (0x28); // 1-Axis Acceleration Data Low Register
        const byte LIS3DHTR_REG_ACCEL_OUT_ADC1_H = (0x29); // 1-Axis Acceleration Data High Register
        const byte LIS3DHTR_REG_ACCEL_OUT_ADC2_L = (0x2A); // 2-Axis Acceleration Data Low Register
        const byte LIS3DHTR_REG_ACCEL_OUT_ADC2_H = (0x2B); // 2-Axis Acceleration Data High Register
        const byte LIS3DHTR_REG_ACCEL_OUT_ADC3_L = (0x2C); // 3-Axis Acceleration Data Low Register
        const byte LIS3DHTR_REG_ACCEL_OUT_ADC3_H = (0x2D); // 3-Axis Acceleration Data High Register
        const byte LIS3DHTR_REG_ACCEL_WHO_AM_I = (0x0F); // Device identification Register
        const byte LIS3DHTR_REG_TEMP_CFG = (0x1F); // Temperature Sensor Register
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1 = (0x20); // Accelerometer Control Register 1
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG2 = (0x21); // Accelerometer Control Register 2
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG3 = (0x22); // Accelerometer Control Register 3
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4 = (0x23); // Accelerometer Control Register 4
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG5 = (0x24); // Accelerometer Control Register 5
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG6 = (0x25); // Accelerometer Control Register 6
        const byte LIS3DHTR_REG_ACCEL_REFERENCE = (0x26); // Reference/Datacapture Register
        const byte LIS3DHTR_REG_ACCEL_STATUS2 = (0x27); // Status Register 2
        const byte LIS3DHTR_REG_ACCEL_OUT_X_L = (0x28); // X-Axis Acceleration Data Low Register
        const byte LIS3DHTR_REG_ACCEL_OUT_X_H = (0x29); // X-Axis Acceleration Data High Register
        const byte LIS3DHTR_REG_ACCEL_OUT_Y_L = (0x2A); // Y-Axis Acceleration Data Low Register
        const byte LIS3DHTR_REG_ACCEL_OUT_Y_H = (0x2B); // Y-Axis Acceleration Data High Register
        const byte LIS3DHTR_REG_ACCEL_OUT_Z_L = (0x2C); // Z-Axis Acceleration Data Low Register
        const byte LIS3DHTR_REG_ACCEL_OUT_Z_H = (0x2D); // Z-Axis Acceleration Data High Register
        const byte LIS3DHTR_REG_ACCEL_FIFO_CTRL = (0x2E); // FIFO Control Register
        const byte LIS3DHTR_REG_ACCEL_FIFO_SRC = (0x2F); // FIFO Source Register
        const byte LIS3DHTR_REG_ACCEL_INT1_CFG = (0x30); // Interrupt Configuration Register
        const byte LIS3DHTR_REG_ACCEL_INT1_SRC = (0x31); // Interrupt Source Register
        const byte LIS3DHTR_REG_ACCEL_INT1_THS = (0x32); // Interrupt Threshold Register
        const byte LIS3DHTR_REG_ACCEL_INT1_DURATION = (0x33); // Interrupt Duration Register
        const byte LIS3DHTR_REG_ACCEL_CLICK_CFG = (0x38); // Interrupt Click Recognition Register
        const byte LIS3DHTR_REG_ACCEL_CLICK_SRC = (0x39); // Interrupt Click Source Register
        const byte LIS3DHTR_REG_ACCEL_CLICK_THS = (0x3A); // Interrupt Click Threshold Register
        const byte LIS3DHTR_REG_ACCEL_TIME_LIMIT = (0x3B); // Click Time Limit Register
        const byte LIS3DHTR_REG_ACCEL_TIME_LATENCY = (0x3C); // Click Time Latency Register
        const byte LIS3DHTR_REG_ACCEL_TIME_WINDOW = (0x3D); // Click Time Window Register

        /**************************************************************************
            TEMPERATURE REGISTER DESCRIPTION
        **************************************************************************/
        const byte LIS3DHTR_REG_TEMP_ADC_PD_MASK = (0x80); // ADC Power Enable Status
        const byte LIS3DHTR_REG_TEMP_ADC_PD_DISABLED = (0x00); // ADC Disabled
        const byte LIS3DHTR_REG_TEMP_ADC_PD_ENABLED = (0x80); // ADC Enabled

        const byte LIS3DHTR_REG_TEMP_TEMP_EN_MASK = (0x40); // Temperature Sensor (T) Enable Status
        const byte LIS3DHTR_REG_TEMP_TEMP_EN_DISABLED = (0x00); // Temperature Sensor (T) Disabled
        const byte LIS3DHTR_REG_TEMP_TEMP_EN_ENABLED = (0x40); // Temperature Sensor (T) Enabled

        /**************************************************************************
            ACCELEROMETER CONTROL REGISTER 1 DESCRIPTION
        **************************************************************************/
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_MASK = (0xF0); // Acceleration Data Rate Selection
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_PD = (0x00); // Power-Down Mode
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_1 = (0x10); // Normal / Low Power Mode (1 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_10 = (0x20); // Normal / Low Power Mode (10 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_25 = (0x30); // Normal / Low Power Mode (25 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_50 = (0x40); // Normal / Low Power Mode (50 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_100 = (0x50); // Normal / Low Power Mode (100 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_200 = (0x60); // Normal / Low Power Mode (200 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_400 = (0x70); // Normal / Low Power Mode (400 Hz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_1_6K = (0x80); // Low Power Mode (1.6 KHz)
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_5K = (0x90); // Normal (1.25 KHz) / Low Power Mode (5 KHz)

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_MASK = (0x08); // Low Power Mode Enable
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_NORMAL = (0x00); // Normal Mode
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_LOW = (0x08); // Low Power Mode

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_MASK = (0x04); // Acceleration Z-Axis Enable
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_DISABLE = (0x00); // Acceleration Z-Axis Disabled
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_ENABLE = (0x04); // Acceleration Z-Axis Enabled

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_MASK = (0x02); // Acceleration Y-Axis Enable
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_DISABLE = (0x00); // Acceleration Y-Axis Disabled
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_ENABLE = (0x02); // Acceleration Y-Axis Enabled

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_MASK = (0x01); // Acceleration X-Axis Enable
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_DISABLE = (0x00); // Acceleration X-Axis Disabled
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_ENABLE = (0x01); // Acceleration X-Axis Enabled

        /**************************************************************************
            ACCELEROMETER CONTROL REGISTER 4 DESCRIPTION
        **************************************************************************/
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_MASK = (0x80); // Block Data Update
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_CONTINUOUS = (0x00); // Continuous Update
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_NOTUPDATED = (0x80); // Output Registers Not Updated until MSB and LSB Read

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_MASK = (0x40); // Big/Little Endian Data Selection
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_LSB = (0x00); // Data LSB @ lower address
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_MSB = (0x40); // Data MSB @ lower address

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_MASK = (0x30); // Full-Scale Selection
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_2G = (0x00); // +/- 2G
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_4G = (0x10); // +/- 4G
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_8G = (0x20); // +/- 8G
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_16G = (0x30); // +/- 16G

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_MASK = (0x08); // High Resolution Output Mode
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_DISABLE = (0x00); // High Resolution Disable
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_ENABLE = (0x08); // High Resolution Enable

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_MASK = (0x06); // Self-Test Enable
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_NORMAL = (0x00); // Normal Mode
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_0 = (0x02); // Self-Test 0
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_1 = (0x04); // Self-Test 1

        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_MASK = (0x01); // SPI Serial Interface Mode Selection
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_4WIRE = (0x00); // 4-Wire Interface
        const byte LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_3WIRE = (0x01); // 3-Wire Interface

        const byte LIS3DHTR_REG_ACCEL_STATUS2_UPDATE_MASK = (0x08); // Has New Data Flag Mask

    }
}
