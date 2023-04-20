from DUE.DUEController import DUEController
import array
import time
from enum import Enum


class MMC56x3Controller:         
    _odr_cache = 0
    _ctrl2_cache = 0

    def __init__(self, dueController: DUEController, slaveAddress = 0x30 ) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        self.__Reset()
     
    def __WriteRegister(self, register: int, value: int) :      
        data = [register, value] 
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))
    
    def __ReadRegister(self, register: int, readcount: int = 1) -> int:
        dataWrite = [ register ]
        dataRead = [0] *readcount

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, len(dataWrite), dataRead, 0, len(dataRead))

        if (readcount == 1):
            return dataRead[0]
        
        return ((dataRead[0]) | (dataRead[1] << 8) | (dataRead[2] << 16) | (dataRead[3] << 24))
    

        
    def __Reset(self) :
        self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL1_REG, 0x80)

        time.sleep(20/1000)

        self._odr_cache = 0
        self._ctrl2_cache = 0

        self.__MagnetSetReset()
        self.SetContinuousMode(False)

    def __MagnetSetReset(self) :

        self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL0_REG, 0x80) #turn on set bit
        time.sleep(1/1000)
        self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL0_REG, 0x10) # turn on reset bit
        time.sleep(1/1000)
        
    def SetContinuousMode(self, enable: bool) :
        if (enable) :
            self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL0_REG, 0x80) # turn on cmm_freq_en bit
            self._ctrl2_cache |= 0x10    # turn on cmm_en bit        
        else :
            self._ctrl2_cache &= ~0x10 # turn off cmm_en bit
        

        self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL2_REG, self._ctrl2_cache)
    def SetDataRate(self, value :int ) :
        if (value < 0 or (value > 255 and value != 1000)):
            raise Exception("value must be in range [0,255] or 1000")

        if (value == 1000) :
            self.__WriteRegister(MMC56x3Controller.MMC5603_ODR_REG, 255) # turn on set bit
            self._ctrl2_cache |= 0x80 #turn on hpower bit

            self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL2_REG, self._ctrl2_cache)
        

        else :
            self.__WriteRegister(MMC56x3Controller.MMC5603_ODR_REG, value) # turn on set bit
            self._ctrl2_cache &= ~0x80 #turn on hpower bit

            self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL2_REG, self._ctrl2_cache)
        
    def get_continuous(self)    :
        return  (self._ctrl2_cache & 0x10) != 0
    
    def set_continuous(self, value: bool):
        return
    
    IsContinuousMode = property(get_continuous, set_continuous)

    def get_temperature(self):
        if (self.IsContinuousMode):
            raise Exception("Reading temperature does not support continuous mode")

        self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL0_REG, 0x02)# TM_T trigger

        while (not self.__CheckStatusBit(1, 7)):
            time.sleep(5/1000)

        temp = self.__ReadRegister(MMC56x3Controller.MMC56X3_OUT_TEMP)

        temp *= 0.8 #  0.8*C / LSB
        temp -= 70  #  0 value is -75
        return temp
    
    def set_temperature(self, value: int):
        return
    
    Temperature = property(get_temperature, set_temperature)

    def __Update(self):
        if (not self.IsContinuousMode) :
            self.__WriteRegister(MMC56x3Controller.MMC56X3_CTRL0_REG, 0x01)

            while (not self.__CheckStatusBit(1, 6)):
                time.sleep(5/1000)

        dataWrite = [ MMC56x3Controller.MMC56X3_OUT_X_L ]
        dataRead = [0] * 9

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, 1, dataRead, 0, 9)

        self.x = (dataRead[0] << 12) | (dataRead[1] << 4) | (dataRead[6] >> 4)
        self.y = (dataRead[2] << 12) | (dataRead[3] << 4) | (dataRead[7] >> 4)
        self.z = (dataRead[4] << 12) | (dataRead[5] << 4) | (dataRead[8] >> 4)

        self.x -= (1 << 19)
        self.y -= (1 << 19)
        self.z -= (1 << 19)
      

    def __CheckStatusBit(self, bit: int, shift: int) :
        val = self.__ReadRegister(MMC56x3Controller.MMC56X3_STATUS_REG, 4)

        val >>= shift

        if ((val & ((1 << (bit)) - 1)) != 0):
            return True

        return False
    
    def get_x(self):
        self.__Update()
        return self.x * 0.00625
    
    def set_x(self, value: int):
        return
    
    X = property(get_x, set_x)

    def get_y(self):
        self.__Update()
        return self.y * 0.00625
    
    def set_y(self, value: int):
        return
    
    Y = property(get_y, set_y)

    def get_z(self):
        self.__Update()
        return self.z * 0.00625
    
    def set_z(self, value: int):
        return
    
    Z = property(get_z, set_z)

    MMC56X3_PRODUCT_ID = 0x39
    MMC56X3_CTRL0_REG = 0x1B
    MMC56X3_CTRL1_REG = 0x1C
    MMC56X3_CTRL2_REG = 0x1D
    MMC56X3_STATUS_REG = 0x18
    MMC56X3_OUT_TEMP = 0x09
    MMC56X3_OUT_X_L = 0x00
    MMC5603_ODR_REG = 0x1A

    MMC56X3_CHIP_ID = 0x10

        
          
    

        
        

        

        





