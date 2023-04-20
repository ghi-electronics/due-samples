from DUE.DUEController import DUEController
import time


class HDC1000Controller: 

    def __init__(self, dueController: DUEController, slaveAddress: int = 0x40) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        config = HDC1000Controller.HDC1000_BOTH_TEMP_HUMI | HDC1000Controller.HDC1000_TEMP_HUMI_14BIT | HDC1000Controller.HDC1000_HEAT_ON | HDC1000Controller.HDC1000_RST
        self.__SetConfig(config)

    

    def __WriteRegister(self, register: int) :       
        data = [register]
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))

        time.sleep(20/1000)
    
    def __ReadRegister(self) -> int:
        dataRead = [0] *2

        self.dueController.I2c.Read(self.slaveAddress, dataRead, 0, len(dataRead))

        value = (dataRead[0] << 8 | dataRead[1]) & 0xFFFF

        return value
    
    def __SetConfig(self, config: int) :
        data = [HDC1000Controller.HDC1000_CONFIG, config, 0]
        
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))

    def get_temperature(self):
        self.__WriteRegister(HDC1000Controller.HDC1000_TEMP)
        raw = self.__ReadRegister() * 1.0

        return raw / 65536.0 * 165.0 - 40.0

    def set_temperature(self, value: int):
        return

    Temperature = property(get_temperature, set_temperature)    

    def get_humidity(self):
        self.__WriteRegister(HDC1000Controller.HDC1000_HUMI)
        raw = self.__ReadRegister() * 1.0

        return raw / 65536.0 * 100.0

    def set_humidity(self, value: int):
        return

    Humidity = property(get_humidity, set_humidity)    

    HDC1000_ADDR = 0x40

    HDC1000_TEMP = 0x00
    HDC1000_HUMI = 0x01
    HDC1000_CONFIG = 0x02

    HDC1000_SERID_1 = 0xFB
    HDC1000_SERID_2 = 0xFC
    HDC1000_SERID_3 = 0xFD
    HDC1000_MFID = 0xFE
    HDC1000_DEVID = 0xFF

    HDC1000_RST = 0x80
    HDC1000_HEAT_ON = 0x20
    HDC1000_HEAT_OFF = 0x00
    HDC1000_BOTH_TEMP_HUMI = 0x10
    HDC1000_SINGLE_MEASUR = 0x00
    HDC1000_TEMP_HUMI_14BIT = 0x00
    HDC1000_TEMP_11BIT = 0x40
    HDC1000_HUMI_11BIT = 0x01
    HDC1000_HUMI_8BIT = 0x02
        
            
    

        
        

            
        
        
        
        
     
     
    
        
        
    
        
        
          
    

        
        

        

        





