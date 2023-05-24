from DUELink.DUELinkController import DUELinkController
import array
import time
from enum import Enum


class AccelG248Controller:         

    def __init__(self, dueController: DUELinkController, slaveAddress = 0x1C ) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        self.__WriteRegister(0x2A, 1)
     
    def __WriteRegister(self, register: int, value: int) :      
        data = [register, value] 
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))
    
    def __ReadRegister(self, register: int, readcount: int) -> bytearray:
        dataWrite = [ register ]
        dataRead = [0] *readcount

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, len(dataWrite), dataRead, 0, len(dataRead))
        return dataRead
        
    def get_x(self)->float:
        read = self.__ReadRegister(0x01, 2)
        v = int (read[0] << 2) | (read[1] >> 6) 

        if (v > 511):
            v = v- 1024

        return v

    
    def set_x(self):
        return
    
    X = property(get_x, set_x)

    def get_y(self)->float:
        read = self.__ReadRegister(0x03, 2)
        v = int (read[0] << 2) | (read[1] >> 6) 

        if (v > 511):
            v = v- 1024

        return v
    
    def set_y(self):
        return
    
    Y = property(get_y, set_y)

    def get_z(self)->float:        
        read = self.__ReadRegister(0x05, 2)
        v = int (read[0] << 2) | (read[1] >> 6) 

        if (v > 511):
            v = v- 1024

        return v
    
    def set_z(self):
        return
    
    Z = property(get_z, set_z)
        
        
          
    

        
        

        

        





