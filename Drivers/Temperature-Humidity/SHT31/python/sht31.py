from DUELink.DUELinkController import DUELinkController
import time


class SHT31Controller: 


    def __init__(self, dueController: DUELinkController, slaveAddress: int = 0x44) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        self.__fastmode = False

    def get_fastmode(self):
        return self.__fastmode

    def set_fastmode(self, value: bool):
        self.__fastmode = value

    FastMode = property(get_fastmode, set_fastmode)

    def __WriteCommand(self, command: int) :       
        data = [command >> 8, command & 0xFF]

        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))

        time.sleep(20/1000)
    
    def __ReadBytes(self, count: int) -> bytearray:
        data = [0] *count

        self.dueController.I2c.Read(self.slaveAddress, data, 0, len(data))        

        return data
    
    def Reset(self, hardreset: bool):
        if hardreset == True:            
            self.WriteCommand(SHT31Controller.SHT31_HARD_RESET )
        else:
            self.WriteCommand(SHT31Controller.SHT31_SOFT_RESET)

        time.sleep(1/1000)

    def __Measurement(self):
        if (self.__fastmode):
            self.__WriteCommand(SHT31Controller.SHT31_MEASUREMENT_FAST )
            time.sleep(4/1000)
        else:
            self.__WriteCommand(SHT31Controller.SHT31_MEASUREMENT_SLOW)
            time.sleep(15/1000)

    def get_temperature(self):
        mA = -45
        mB = 175
        mC = 65535

        self.__Measurement()

        buffer = self.__ReadBytes(6)

        t = float((buffer[0] << 8) + buffer[1])

        return mA + mB * (t / mC)
    def set_temperature(self, value: int):
        return
    
    Temperature = property(get_temperature, set_temperature)

    def get_humidity(self):
       
        mX = 0
        mY = 100
        mZ = 65535

        self.__Measurement()

        buffer = self.__ReadBytes(6)

        h = float((buffer[3] << 8) + buffer[4])

        return mX + mY * (h / mZ)
    
    def set_humidity(self, value: int):
        return
    
    Humidity = property(get_humidity, set_humidity)

    SHT31_SOFT_RESET = 0x30A2
    SHT31_HARD_RESET = 0x0006

    SHT31_MEASUREMENT_FAST = 0x2416
    SHT31_MEASUREMENT_SLOW = 0x2400

    
            
    

        
        

            
        
        
        
        
     
     
    
        
        
    
        
        
          
    

        
        

        

        





