from DUELink.DUELinkController import DUELinkController
import array
import time
import math


class BMP280Controller: 

    BMP280_REG_DIG_T1 = 0x88
    BMP280_REG_DIG_T2 = 0x8A
    BMP280_REG_DIG_T3 = 0x8C

    BMP280_REG_DIG_P1 = 0x8E
    BMP280_REG_DIG_P2 = 0x90
    BMP280_REG_DIG_P3 = 0x92
    BMP280_REG_DIG_P4 = 0x94
    BMP280_REG_DIG_P5 = 0x96
    BMP280_REG_DIG_P6 = 0x98
    BMP280_REG_DIG_P7 = 0x9A
    BMP280_REG_DIG_P8 = 0x9C
    BMP280_REG_DIG_P9 = 0x9E

    BMP280_REG_CHIPID = 0xD0
    BMP280_REG_VERSION = 0xD1
    BMP280_REG_SOFTRESET = 0xE0

    BMP280_REG_CONTROL = 0xF4
    BMP280_REG_CONFIG = 0xF5
    BMP280_REG_PRESSUREDATA = 0xF7
    BMP280_REG_TEMPDATA = 0xFA

    MSL = 102009 # Mean Sea Level in Pa

    def __init__(self, dueController: DUELinkController, slaveAddress = 0x77 ) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress

        chip_id = self.__ReadRegister8(BMP280Controller.BMP280_REG_CHIPID)

        if (chip_id != 0x58):
            raise Exception("The device is not supported.")
        

        self.dig_T1 = self.__ReadRegister16LE(BMP280Controller.BMP280_REG_DIG_T1)
        self.dig_T2 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_T2)
        self.dig_T3 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_T3)
        self.dig_P1 = self.__ReadRegister16LE(BMP280Controller.BMP280_REG_DIG_P1)
        self.dig_P2 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P2)
        self.dig_P3 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P3)
        self.dig_P4 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P4)
        self.dig_P5 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P5)
        self.dig_P6 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P6)
        self.dig_P7 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P7)
        self.dig_P8 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P8)
        self.dig_P9 = self.__ReadRegisterS16LE(BMP280Controller.BMP280_REG_DIG_P9)
        self.__WriteRegister8(BMP280Controller.BMP280_REG_CONTROL, 0x3F)

     
    def __WriteRegister(self, register: int, data: bytearray) :       
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))

    def __WriteRegister8(self, register: int, value: int) :  
        dataWrite = [value]     
        self.__WriteRegister(register,dataWrite)
    
    def __ReadRegister(self, register: int, readcount: int) -> bytearray:
        dataWrite = [ register ]
        dataRead = [0] *readcount

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, len(dataWrite), dataRead, 0, len(dataRead))

        return dataRead
    
    def __ReadRegister8(self, register: int)-> int:
        data = self.__ReadRegister(register, 1)

        return data[0] & 0xFF
    
    def __ReadRegister16(self, register: int)-> int:
        data = self.__ReadRegister(register, 2)

        return (data[0] << 8 | data[1]) & 0xFFFF
    
    def __ReadRegister16LE(self,  register: int) -> int:
        data = self.__ReadRegister16(register)
        return ((data >> 8) | (data << 8)) & 0xFFFF
    
    def __ReadRegisterS16LE(self, register: int)-> int:
        s = self.__ReadRegister16LE(register)
        return BMP280Controller.__unsignedToSigned(s, 2)
        
    def __ReadRegister24(self, register: int)-> int:
        data = self.__ReadRegister(register, 3)

        return (data[0] << 16 | data[1] << 8 | data[2]) & 0xFFFFFF
    
    def __unsignedToSigned(n, byte_count): 
        return int.from_bytes(n.to_bytes(byte_count, 'little', signed=False), 'little', signed=True)
    
    def __signedToUnsigned(n, byte_count): 
        return int.from_bytes(n.to_bytes(byte_count, 'little', signed=True), 'little', signed=False)
        
    def __CalcAltitude(self, p0: float, p1:float, t:float):
        c = (p0 / p1)
        c = math.pow(c, (1 / 5.25588)) - 1.0
        c = (c * (t + 273.15)) / 0.0065
        return c
    
    def CalculateAltitude(self) :
        t = self.GetTemperature()
        p1 = self.GetPressure()
        return self.__CalcAltitude(BMP280Controller.MSL, p1, t)
        
    def GetTemperature(self):
       
        adc_T = self.__ReadRegister24(BMP280Controller.BMP280_REG_TEMPDATA)

        adc_T >>= 4
        var1 = (((adc_T >> 3) - ((self.dig_T1 << 1))) *
                (self.dig_T2)) >> 11
        var2 = (((((adc_T >> 4) - (self.dig_T1)) *
                    ((adc_T >> 4) - (self.dig_T1))) >> 12) *
                (self.dig_T3)) >> 14
        self.fine = var1 + var2
        t =((self.fine * 5 + 128) >> 8)
        return t / 100
        
    def GetPressure(self) :
        
        # Call getTemperature to get t_fine
        self.GetTemperature()

        adc_P = self.__ReadRegister24(BMP280Controller.BMP280_REG_PRESSUREDATA)
       
       
        adc_P >>= 4
        var1 = (self.fine) - 128000

       
        var2 = var1 * var1 * (self.dig_P6) 
       
        var2 = (var2 + ((var1 * self.dig_P5) << 17)) 

        var2 = (var2 + ((self.dig_P4) << 35) )
        
        var1 = ((var1 * var1 * self.dig_P3) >> 8) + ((var1 * self.dig_P2) << 12)
        var1 = ((((1) << 47) + var1)) * (self.dig_P1) >> 33
        if (var1 == 0) :
            return 0 # avoid exception caused by division by zero
        
        #print(f"{var1}, {var2}")

        p = 1048576 - adc_P
        p = int((((p << 31) - var2) * 3125) / var1)
        var1 = ((self.dig_P9) * (p >> 13) * (p >> 13)) >> 25
        var2 = ((self.dig_P8) * p) >> 19
        p = ((p + var1 + var2) >> 8) + ((self.dig_P7) << 4)
        return int(p / 256)
        
        
    
        
        
          
    

        
        

        

        





