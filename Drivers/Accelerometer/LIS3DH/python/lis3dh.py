from DUELink.DUELinkController import DUELinkController
import array
import time
from enum import Enum


class LIS3DHController: 

    WHO_AM_I = 0b00110011
    LIS3DHTR_CONVERSIONDELAY = 100

    #
    # ACCELEROMETER REGISTERS
    #
    LIS3DHTR_REG_ACCEL_STATUS = (0x07)  # Status Register
    LIS3DHTR_REG_ACCEL_OUT_ADC1_L = (0x28)  # 1-Axis Acceleration Data Low Register
    LIS3DHTR_REG_ACCEL_OUT_ADC1_H = (0x29)  # 1-Axis Acceleration Data High Register
    LIS3DHTR_REG_ACCEL_OUT_ADC2_L = (0x2A)  # 2-Axis Acceleration Data Low Register
    LIS3DHTR_REG_ACCEL_OUT_ADC2_H = (0x2B)  # 2-Axis Acceleration Data High Register
    LIS3DHTR_REG_ACCEL_OUT_ADC3_L = (0x2C)  # 3-Axis Acceleration Data Low Register
    LIS3DHTR_REG_ACCEL_OUT_ADC3_H = (0x2D)  # 3-Axis Acceleration Data High Register
    LIS3DHTR_REG_ACCEL_WHO_AM_I = (0x0F)  # Device identification Register
    LIS3DHTR_REG_TEMP_CFG = (0x1F)  # Temperature Sensor Register
    LIS3DHTR_REG_ACCEL_CTRL_REG1 = (0x20)  # Accelerometer Control Register 1
    LIS3DHTR_REG_ACCEL_CTRL_REG2 = (0x21)  # Accelerometer Control Register 2
    LIS3DHTR_REG_ACCEL_CTRL_REG3 = (0x22)  # Accelerometer Control Register 3
    LIS3DHTR_REG_ACCEL_CTRL_REG4 = (0x23)  # Accelerometer Control Register 4
    LIS3DHTR_REG_ACCEL_CTRL_REG5 = (0x24)  # Accelerometer Control Register 5
    LIS3DHTR_REG_ACCEL_CTRL_REG6 = (0x25)  # Accelerometer Control Register 6
    LIS3DHTR_REG_ACCEL_REFERENCE = (0x26)  # Reference/Datacapture Register
    LIS3DHTR_REG_ACCEL_STATUS2 = (0x27)  # Status Register 2
    LIS3DHTR_REG_ACCEL_OUT_X_L = (0x28)  # X-Axis Acceleration Data Low Register
    LIS3DHTR_REG_ACCEL_OUT_X_H = (0x29)  # X-Axis Acceleration Data High Register
    LIS3DHTR_REG_ACCEL_OUT_Y_L = (0x2A)  # Y-Axis Acceleration Data Low Register
    LIS3DHTR_REG_ACCEL_OUT_Y_H = (0x2B)  # Y-Axis Acceleration Data High Register
    LIS3DHTR_REG_ACCEL_OUT_Z_L = (0x2C)  # Z-Axis Acceleration Data Low Register
    LIS3DHTR_REG_ACCEL_OUT_Z_H = (0x2D)  # Z-Axis Acceleration Data High Register
    LIS3DHTR_REG_ACCEL_FIFO_CTRL = (0x2E)  # FIFO Control Register
    LIS3DHTR_REG_ACCEL_FIFO_SRC = (0x2F)  # FIFO Source Register
    LIS3DHTR_REG_ACCEL_INT1_CFG = (0x30)  # Interrupt Configuration Register
    LIS3DHTR_REG_ACCEL_INT1_SRC = (0x31)  # Interrupt Source Register
    LIS3DHTR_REG_ACCEL_INT1_THS = (0x32)  # Interrupt Threshold Register
    LIS3DHTR_REG_ACCEL_INT1_DURATION = (0x33)  # Interrupt Duration Register
    LIS3DHTR_REG_ACCEL_CLICK_CFG = (0x38)  # Interrupt Click Recognition Register
    LIS3DHTR_REG_ACCEL_CLICK_SRC = (0x39)  # Interrupt Click Source Register
    LIS3DHTR_REG_ACCEL_CLICK_THS = (0x3A)  # Interrupt Click Threshold Register
    LIS3DHTR_REG_ACCEL_TIME_LIMIT = (0x3B)  # Click Time Limit Register
    LIS3DHTR_REG_ACCEL_TIME_LATENCY = (0x3C)  # Click Time Latency Register
    LIS3DHTR_REG_ACCEL_TIME_WINDOW = (0x3D)  # Click Time Window Register

    #
    #TEMPERATURE REGISTER DESCRIPTION
    #
    LIS3DHTR_REG_TEMP_ADC_PD_MASK = (0x80)  # ADC Power Enable Status
    LIS3DHTR_REG_TEMP_ADC_PD_DISABLED = (0x00)  # ADC Disabled
    LIS3DHTR_REG_TEMP_ADC_PD_ENABLED = (0x80)  # ADC Enabled

    LIS3DHTR_REG_TEMP_TEMP_EN_MASK = (0x40)  # Temperature Sensor (T) Enable Status
    LIS3DHTR_REG_TEMP_TEMP_EN_DISABLED = (0x00)  # Temperature Sensor (T) Disabled
    LIS3DHTR_REG_TEMP_TEMP_EN_ENABLED = (0x40)  # Temperature Sensor (T) Enabled

    #
    # ACCELEROMETER CONTROL REGISTER 1 DESCRIPTION
    #
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_MASK = (0xF0)  # Acceleration Data Rate Selection
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_PD = (0x00)  # Power-Down Mode
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_1 = (0x10)  # Normal / Low Power Mode (1 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_10 = (0x20)  # Normal / Low Power Mode (10 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_25 = (0x30)  # Normal / Low Power Mode (25 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_50 = (0x40)  # Normal / Low Power Mode (50 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_100 = (0x50)  # Normal / Low Power Mode (100 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_200 = (0x60)  # Normal / Low Power Mode (200 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_400 = (0x70)  # Normal / Low Power Mode (400 Hz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_1_6K = (0x80)  # Low Power Mode (1.6 KHz)
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_5K = (0x90)  # Normal (1.25 KHz) / Low Power Mode (5 KHz)

    LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_MASK = (0x08)  # Low Power Mode Enable
    LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_NORMAL = (0x00)  # Normal Mode
    LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_LOW = (0x08)  # Low Power Mode

    LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_MASK = (0x04)  # Acceleration Z-Axis Enable
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_DISABLE = (0x00)  # Acceleration Z-Axis Disabled
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_ENABLE = (0x04)  # Acceleration Z-Axis Enabled

    LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_MASK = (0x02)  # Acceleration Y-Axis Enable
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_DISABLE = (0x00)  # Acceleration Y-Axis Disabled
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_ENABLE = (0x02)  # Acceleration Y-Axis Enabled

    LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_MASK = (0x01)  # Acceleration X-Axis Enable
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_DISABLE = (0x00)  # Acceleration X-Axis Disabled
    LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_ENABLE = (0x01)  # Acceleration X-Axis Enabled

    #
    # ACCELEROMETER CONTROL REGISTER 4 DESCRIPTION
    #
    LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_MASK = (0x80)  # Block Data Update
    LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_CONTINUOUS = (0x00)  # Continuous Update
    LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_NOTUPDATED = (0x80)  # Output Registers Not Updated until MSB and LSB Read

    LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_MASK = (0x40)  # Big/Little Endian Data Selection
    LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_LSB = (0x00)  # Data LSB @ lower address
    LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_MSB = (0x40)  # Data MSB @ lower address

    LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_MASK = (0x30)  # Full-Scale Selection
    LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_2G = (0x00)  # +/- 2G
    LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_4G = (0x10)  # +/- 4G
    LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_8G = (0x20)  # +/- 8G
    LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_16G = (0x30)  # +/- 16G

    LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_MASK = (0x08)  # High Resolution Output Mode
    LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_DISABLE = (0x00)  # High Resolution Disableself.
    LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_ENABLE = (0x08)  # High Resolution Enable

    LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_MASK = (0x06)  # Self-Test Enable
    LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_NORMAL = (0x00)  # Normal Mode
    LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_0 = (0x02)  # Self-Test 0
    LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_1 = (0x04)  # Self-Test 1

    LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_MASK = (0x01)  # SPI Serial Interface Mode Selection
    LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_4WIRE = (0x00)  # 4-Wire Interface
    LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_3WIRE = (0x01)  # 3-Wire Interface

    LIS3DHTR_REG_ACCEL_STATUS2_UPDATE_MASK = (0x08)  # Has New Data Flag Mask    

    def __init__(self, dueController: DUELinkController, slaveAddress = 0x19 ) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress

        whoami = self.__ReadRegister8(0xf)

        if (whoami != LIS3DHController.WHO_AM_I):
            raise Exception("The device is not supported")
        
        config5 = LIS3DHController.LIS3DHTR_REG_TEMP_ADC_PD_ENABLED | LIS3DHController.LIS3DHTR_REG_TEMP_TEMP_EN_DISABLED

        self.__WriteRegister8(LIS3DHController.LIS3DHTR_REG_TEMP_CFG,config5)

        time.sleep(LIS3DHController.LIS3DHTR_CONVERSIONDELAY/1000)

        config1 = LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1_LPEN_NORMAL  # Normal Mode
        config1 |= LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1_AZEN_ENABLE  # Acceleration Z-Axis Enabled
        config1 |= LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1_AYEN_ENABLE  # Acceleration Y-Axis Enabled
        config1 |= LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1_AXEN_ENABLE

        self.__WriteRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1, config1)

        time.sleep(LIS3DHController.LIS3DHTR_CONVERSIONDELAY/1000)

        config4 = LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_BDU_NOTUPDATED # Continuous Update
        config4 |=            LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_BLE_LSB         # Data LSB @ lower address
        config4 |=            LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_DISABLE      # High Resolution Disable
        config4 |=            LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_ST_NORMAL       # Normal Mode
        config4 |=            LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_SIM_4WIRE;       # 4-Wire Interface

        self.__WriteRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4, config4)

        time.sleep(LIS3DHController.LIS3DHTR_CONVERSIONDELAY/1000)

        self.SetFullScaleRange(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_16G)
        

        self.SetOutputDataRate(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_50)
        self.SetHighSolution(True)
        
     
    def __WriteRegister(self, register: int, data: bytearray) :       
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))
    
    def __ReadRegister(self, register: int, readcount: int) -> bytearray:
        dataWrite = [ register ]
        dataRead = [0] *readcount

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, len(dataWrite), dataRead, 0, len(dataRead))

        return dataRead
    
    def __ReadRegister8(self, register: int):
        data = self.__ReadRegister(register, 1)
        return data[0]
    
    def __WriteRegister8(self, register: int, value: int):
        self.__WriteRegister(register, [value])

    def SetHighSolution(self, enable: bool):
             
        data = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4)

        if (enable):
            data =(data | LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_ENABLE)
        else:
            data = (data & ~LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_HS_ENABLE)
        

        self.__WriteRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4, data)
        return
       
    
    def SetOutputDataRate(self, value: int):        
        data = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1)

        data = (data & ~LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1_AODR_MASK)
        data = (data | value)

        self.__WriteRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG1, data)
        time.sleep(LIS3DHController.LIS3DHTR_CONVERSIONDELAY/1000)
        
   
    def SetFullScaleRange(self, value: int) :        
        data = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4)

        data = (data & (~LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_MASK))
        data = (data | value)

        self.__WriteRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4, data)

        time.sleep(LIS3DHController.LIS3DHTR_CONVERSIONDELAY/1000)

        match (value) :
            case LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_16G:
                self.accRange = 1280

            case LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_8G:
                self.accRange = 3968
        
            case LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_4G:
                self.accRange = 7282
   
            case LIS3DHController.LIS3DHTR_REG_ACCEL_CTRL_REG4_FS_2G:
                self.accRange = 16000
     
            case _:
                return
        
     
        
    def get_x(self)->float:
        xAccelLo = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_OUT_X_L)
        xAccelHi = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_OUT_X_H)
        v = ((xAccelHi << 8) | xAccelLo)

        return v / self.accRange

    
    def set_x(self):
        return
    
    X = property(get_x, set_x)

    def get_y(self)->float:
        xAccelLo = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_OUT_Y_L)
        xAccelHi = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_OUT_Y_H)
        v = ((xAccelHi << 8) | xAccelLo)

        return v / self.accRange
    
    def set_y(self):
        return
    
    Y = property(get_y, set_y)

    def get_z(self)->float:        
        xAccelLo = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_OUT_Z_L)        
        xAccelHi = self.__ReadRegister8(LIS3DHController.LIS3DHTR_REG_ACCEL_OUT_Z_H)        
        v = ((xAccelHi << 8) | xAccelLo)
        return v / self.accRange
    
    def set_z(self):
        return
    
    Z = property(get_z, set_z)
        
        
          
    

        
        

        

        





