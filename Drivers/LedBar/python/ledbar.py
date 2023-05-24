from DUELink.DUELinkController import DUELinkController
from DUELink.Digital import Input
import time
import math


class LedBarController: 
    INPUT = 0
    OUTPUT = 1

    def __init__(self, dueController: DUELinkController, pinClk: int, pinData: int) -> None:
        self.dueController = dueController
        self.pinClk = pinClk
        self.pinData = pinData
        self.__reverseShow = False
        self.__lednum = 10
        self.leds = [0] * self.__lednum

        self.__PinMode(self.pinClk, LedBarController.OUTPUT)
        self.__PinMode(self.pinData, LedBarController.OUTPUT)

    def get_lednums(self):
        return self.__lednum
    
    def set_lednums(self, num: int) :
        return        
    
    LedNum = property(get_lednums, set_lednums)

    def get_reverseshow(self):
        return self.__reverseShow
    
    def set_reverseshow(self, value: int) :
        self.__reverseShow = value

    ReverseShow = property(get_reverseshow, set_reverseshow)

    def __PinMode(self, pin: int, mode: int) :
        if (mode == LedBarController.OUTPUT):
            self.dueController.Digital.Write(pin, False)
        else :
            self.dueController.Digital.Read(pin, Input.PULL_NONE)
    def __Latch(self):
        self.dueController.Digital.Write(self.pinData, False)
        self.dueController.Digital.Write(self.pinClk, True)
        self.dueController.Digital.Write(self.pinClk, False)
        self.dueController.Digital.Write(self.pinClk, True) 
        self.dueController.Digital.Write(self.pinClk, False)
        time.sleep(240/1000000)
        
        self.dueController.Digital.Write(self.pinData, True) 
        self.dueController.Digital.Write(self.pinData, False)
        self.dueController.Digital.Write(self.pinData, True) 
        self.dueController.Digital.Write(self.pinData, False)
        self.dueController.Digital.Write(self.pinData, True) 
        self.dueController.Digital.Write(self.pinData, False)
        self.dueController.Digital.Write(self.pinData, True) 
        self.dueController.Digital.Write(self.pinData, False)
        time.sleep(1/1000000)
        
        self.dueController.Digital.Write(self.pinClk, True)
        self.dueController.Digital.Write(self.pinClk, False)
        

    def __SendBits(self, bits: int):
        clk = False

        for i in range(16)    :
            if (bits & 0x8000) != 0:
                self.dueController.Digital.Write(self.pinData, True)
            else:
                self.dueController.Digital.Write(self.pinData, False)
            
            self.dueController.Digital.Write(self.pinClk, clk)
            clk = not clk
            bits <<= 1

    def __Send(self):
        if (self.__reverseShow):
            
            self.__SendBits(0x00); #send cmd(0x00)

            for i in range(self.__lednum, 0, -1):
                self.__SendBits(self.leds[i-1])
            
            for i in range (0, 12 - self.__lednum):
                self.__SendBits(0x00)
           
        else :
            self.__SendBits(0x00); #send cmd(0x00)

            for i in range(0, self.__lednum):
                self.__SendBits(self.leds[i])

            for i in range (0, 12 - self.__lednum):
                self.__SendBits(0x00)            
    
    def SetLed(self, ledId: int, brightness: int):
            if ledId > self.__lednum:
                ledId = self.__lednum            

            if brightness > 255:
                brightness = 255            

            self.leds[ledId] = brightness

    def Show(self):
        self.__Send()
        self.__Latch()

    def Clear(self):
        for i in range (self.__lednum):
            self.leds[i] = 0

        
        

            
        
        
        
        
     
     
    
        
        
    
        
        
          
    

        
        

        

        





