from DUE.DUEController import DUEController
import time


class APA102Controller: 


    def __init__(self, dueController: DUEController, slaveAddress: int = 0x23) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        self.__maxled = 10
    
    def get_ledcount(self)   :
        return self.__maxled
    
    def set_ledcount(self, value: int):
        if value > 155:
            raise Exception("Support max 255 leds.")
        
        data = [APA102Controller.COMMAND_CHANGE_LED_LENGTH, value ]
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))
        __maxled = value

    LedCount = property(get_ledcount, set_ledcount)
        
    
    def Set(self, ledIdx: int, red: int, green: int, blue: int)    :
        if ledIdx < 0 or ledIdx > self.__maxled:
            raise Exception("ledIndex is out of range.")
    
        ledIdx +=1
        #data = bytearray(5)

        #data[0] = APA102Controller.COMMAND_WRITE_SINGLE_LED_COLOR
        #data[1] = ledIdx
        #data[2] = red
        #data[3] = green
        #data[4] = blue

        data = [APA102Controller.COMMAND_WRITE_SINGLE_LED_COLOR, ledIdx, red, green, blue]

        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))

       

    def SetAll(self, red: int, green: int, blue: int):
        data = bytearray(4)

        #data[0] = APA102Controller.COMMAND_WRITE_ALL_LED_COLOR
        #data[1] = red
        #data[2] = green
        #data[3] = blue

        data = [APA102Controller.COMMAND_WRITE_ALL_LED_COLOR, red, green, blue]
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))
       
    
    def Clear(self)    :
        #data = bytearray(1)
        #data[0] = APA102Controller.COMMAND_WRITE_SINGLE_LED_BRIGHTNESS

        data = [APA102Controller.COMMAND_WRITE_SINGLE_LED_BRIGHTNESS]
        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))
       


    def ChangeAddress(self, newAddress: int) :
        if (newAddress < 0x08 or newAddress > 0x77):
            raise Exception("new address must be in range [0x08, 0x77]")

        if (newAddress == self.slaveAddress):
            return

        data = [ APA102Controller.COMMAND_CHANGE_ADDRESS, newAddress ]

        self.dueController.I2c.Write(self.slaveAddress, data, 0, len(data))

        self.slaveAddress = newAddress
       

    COMMAND_CHANGE_ADDRESS = 0xC7
    COMMAND_CHANGE_LED_LENGTH = 0x70
    COMMAND_WRITE_SINGLE_LED_COLOR = 0x71
    COMMAND_WRITE_ALL_LED_COLOR = 0x72
    COMMAND_WRITE_RED_ARRAY = 0x73
    COMMAND_WRITE_GREEN_ARRAY = 0x74
    COMMAND_WRITE_BLUE_ARRAY = 0x75
    COMMAND_WRITE_SINGLE_LED_BRIGHTNESS = 0x76
    COMMAND_WRITE_ALL_LED_BRIGHTNESS = 0x77
    COMMAND_WRITE_ALL_LED_OFF = 0x78 
        
            
    

        
        

            
        
        
        
        
     
     
    
        
        
    
        
        
          
    

        
        

        

        





