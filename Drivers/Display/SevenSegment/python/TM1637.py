from DUELink.DUELinkController import DUELinkController
from DUELink.Digital import Input
import time

class TM1637Controller:

    INPUT = 0
    OUTPUT= 1

    TM1637_I2C_COMM1 = 0x40
    TM1637_I2C_COMM2 = 0xC0
    TM1637_I2C_COMM3 = 0x80

    digitToSegment = [
        # XGFEDCBA
        0b00111111,    # 0
        0b00000110,    # 1
        0b01011011,    # 2
        0b01001111,    # 3
        0b01100110,    # 4
        0b01101101,    # 5
        0b01111101,    # 6
        0b00000111,    # 7
        0b01111111,    # 8
        0b01101111,    # 9
        0b01110111,    # A
        0b01111100,    # b
        0b00111001,    # C
        0b01011110,    # d
        0b01111001,    # E
        0b01110001     # F

    ]

    MinusSegments = 0b01000000

    def __init__(self, dueController: DUELinkController, pinClk: int, pinDio: int) -> None:
        self.__dueController = dueController
        self.__pinClk = pinClk
        self.__pinDio = pinDio
        self.__bitDelay = 0

        self.__dueController.Digital.Read(self.__pinClk, Input.PULL_NONE)
        self.__dueController.Digital.Read(self.__pinDio, Input.PULL_NONE)

        self.__dueController.Digital.Write(self.__pinClk, False)
        self.__dueController.Digital.Write(self.__pinDio, False)

        self.__brightness = 0x0F
        
    def get_brightness(self):
        return (self.__brightness & 0x07)
    
    def set_brightness(self, value: int):
        if value < 0 or value > 7:
            raise Exception("brightness must be in range [0,7]")
        
        if value > 0:
            self.__brightness = 8 # turn on
            self.__brightness |= (value & 0x07)  
        else :
            self.__brightness = 0

                
    Brightness = property(get_brightness, set_brightness)

    def get_bitdelay(self):
        return (self.__bitDelay)
    
    def set_bitdelay(self, value: int):
        self.__bitDelay = value

                
    BitDelay = property(get_bitdelay, set_bitdelay)

    def __Delay(self):
        time.sleep(self.__bitDelay/1000000)
    
    def __PinMode(self, pin, mode):
        if (mode == TM1637Controller.OUTPUT) :
            self.__dueController.Digital.Write(pin, False)
        
        else :
            self.__dueController.Digital.Read(pin, Input.PULL_NONE)
        

    
    def __Start(self):
        self.__PinMode(self.__pinDio, TM1637Controller.OUTPUT)
        self.__Delay()

    def __Stop(self):
        self.__PinMode(self.__pinDio, TM1637Controller.OUTPUT)
        self.__Delay()
        self.__PinMode(self.__pinClk, TM1637Controller.INPUT)
        self.__Delay()
        self.__PinMode(self.__pinDio, TM1637Controller.INPUT)
        self.__Delay()

    def __WriteByte(self, b) :
            data = b

            # 8 Data Bits
            for i in range (0, 8):
                # CLK low
                self.__PinMode(self.__pinClk, TM1637Controller.OUTPUT)
                self.__Delay()

                # Set data bit
                if ((data & 0x01) != 0):
                    self.__PinMode(self.__pinDio, TM1637Controller.INPUT)
                else:
                    self.__PinMode(self.__pinDio, TM1637Controller.OUTPUT)

                self.__Delay()

                # CLK high
                self.__PinMode(self.__pinClk, TM1637Controller.INPUT)
                self.__Delay()
                data = (data >> 1)
            
            # Wait for acknowledge
            # CLK to zero
            self.__PinMode(self.__pinClk, TM1637Controller.OUTPUT)
            self.__PinMode(self.__pinDio, TM1637Controller.INPUT)
            self.__Delay()

            # CLK to high
            self.__PinMode(self.__pinClk, TM1637Controller.INPUT)
            self.__Delay()
            ack = self.__dueController.Digital.Read(self.__pinDio, Input.PULL_NONE)
            if (ack == False):
                self.__PinMode(self.__pinDio, TM1637Controller.OUTPUT)


            self.__Delay()
            self.__PinMode(self.__pinClk, TM1637Controller.OUTPUT)
            self.__Delay()

            return ack
        

    def __EncodeDigit(self, digit):
        return TM1637Controller.digitToSegment[digit & 0x0f]
    
    def ShowNumberDec(self, num: int, dot: bool):
        self.ShowNumberBase(10, num, dot)

    def ShowNumberHex(self, num: int, dot: bool) :
        self.ShowNumberBase(16, num, dot);    
    
    def ShowNumberBase(self, baseNum: int, num: int, dot: bool):
        digits = bytearray(4)

        newPos = 0
        posEnd = 0
        posStart = 0
        posLength  = 4
        
        if num < 0:
            posLength = 3

        newNum = abs(num)

        for i in range (posLength):
            newNum = int(newNum / baseNum)

            if (newNum == 0):
                break
        
            posEnd = posEnd + 1
                
        if (num < 0) :
            posEnd = posEnd + 1
            posStart = posStart + 1
            digits[0] = TM1637Controller.MinusSegments            

        newNum = abs(num)

        for i in range (posEnd, posStart-1,-1 ):

            digit = int(newNum % baseNum)
            newNum = int(newNum / baseNum)

            digits[i] = (self.__EncodeDigit(digit) )  


            if (newNum == 0) :
                break            

        if (dot) :
            digits[1] = (digits[1] | 0x80)
        

        self.__SetSegments(digits, 4, newPos)

    def Clear(self):
        digits = [0] * 4
        self.__SetSegments(digits, 4, 0)

    def __SetSegments(self, segments: bytearray, length: int, pos: int):
            #Write COMM1
            self.__Start()
            self.__WriteByte(TM1637Controller.TM1637_I2C_COMM1)
            self.__Stop()

            #Write COMM2 + first digit address
            self.__Start()
            self.__WriteByte(TM1637Controller.TM1637_I2C_COMM2 + (pos & 0x03))

            #Write the data bytes
            for k in range(length):
                self.__WriteByte(segments[k])

            self.__Stop()

            #Write COMM3 + brightness
            self.__Start()
            self.__WriteByte(TM1637Controller.TM1637_I2C_COMM3 + (self.__brightness & 0x0f))
            self.__Stop()
        

    





