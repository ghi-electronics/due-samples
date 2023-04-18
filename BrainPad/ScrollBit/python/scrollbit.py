from DUE.DUEController import DUEController
from DUE.Digital import Input
import array
import time
import numpy





class ScrollBitController:    

    I2C_ADDR = 0x74
    REG_MODE = 0x00
    REG_FRAME = 0x01
    REG_AUDIOSYNC = 0x06
    REG_SHUTDOWN = 0x0a

    REG_COLOR = 0x24
    REG_BLINK = 0x12
    REG_ENABLE = 0x00

    REG_BANK = 0xfd
    BANK_CONFIG = 0x0b

    ROWS = 7
    COLS = 17
    
    frame = 0

    gamma = [
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2,
        2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5,
        6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 11, 11,
        11, 12, 12, 13, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18,
        19, 19, 20, 21, 21, 22, 22, 23, 23, 24, 25, 25, 26, 27, 27, 28,
        29, 29, 30, 31, 31, 32, 33, 34, 34, 35, 36, 37, 37, 38, 39, 40,
        40, 41, 42, 43, 44, 45, 46, 46, 47, 48, 49, 50, 51, 52, 53, 54,
        55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70,
        71, 72, 73, 74, 76, 77, 78, 79, 80, 81, 83, 84, 85, 86, 88, 89,
        90, 91, 93, 94, 95, 96, 98, 99, 100, 102, 103, 104, 106, 107, 109, 110,
        111, 113, 114, 116, 117, 119, 120, 121, 123, 124, 126, 128, 129, 131, 132, 134,
        135, 137, 138, 140, 142, 143, 145, 146, 148, 150, 151, 153, 155, 157, 158, 160,
        162, 163, 165, 167, 169, 170, 172, 174, 176, 178, 179, 181, 183, 185, 187, 189,
        191, 193, 194, 196, 198, 200, 202, 204, 206, 208, 210, 212, 214, 216, 218, 220,
        222, 224, 227, 229, 231, 233, 235, 237, 239, 241, 244, 246, 248, 250, 252, 255
    ]

    def __init__(self, dueController: DUEController, slaveAddress = 0x74 ) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        self.buf = bytearray(144)
        self.UpSideDown = False
        self.Clear()
        self.__WriteByte( ScrollBitController.REG_BANK, ScrollBitController.BANK_CONFIG);

        time.sleep(1/1000)
        self.__WriteByte( ScrollBitController.REG_SHUTDOWN, 0)
        time.sleep(1/1000)

        self.__WriteByte( ScrollBitController.REG_SHUTDOWN, 1)
        time.sleep(1/1000)


        self.__WriteByte( ScrollBitController.REG_MODE, 0)
        self.__WriteByte( ScrollBitController.REG_AUDIOSYNC, 0)

        enable = bytearray(17)
        for i in range(len(enable)):
            enable[i] = 0xFF       
        
        self.__WriteByte( ScrollBitController.REG_BANK, 0)
        self.__WriteBuffer( 0, enable)
        self.__WriteByte( ScrollBitController.REG_BANK, 1)
        self.__WriteBuffer( 0, enable)


    def __WriteByte(self, register: int, value: int) :
            temp = bytearray(2)
            temp[0] = register & 0xFF
            temp[1] = value & 0xFF   
            self.dueController.I2c.Write(self.slaveAddress,temp, 0, len(temp) )
    
    def __WriteBuffer(self, register: int, value: bytearray):
            temp = bytearray(len(value) + 1 )
            temp[0] = register

            for x in range (len(value)):
                 temp[x + 1] = value[x]
            
            self.dueController.I2c.Write(self.slaveAddress, temp, 0, len(temp))

    def __CorrectGamma(self, brightness: int):
        return self.gamma[brightness];            

    def Show(self):
        corrected_buf = bytearray(len(self.buf))

        for x in range (len(self.buf)):
            corrected_buf[x] = self.__CorrectGamma(self.buf[x])



        self.__WriteByte( ScrollBitController.REG_BANK, self.frame)
        self.__WriteBuffer(ScrollBitController.REG_COLOR, corrected_buf)
        self.__WriteByte( ScrollBitController.REG_BANK, ScrollBitController.BANK_CONFIG)
        self.__WriteByte( ScrollBitController.REG_FRAME, self.frame)

        if self.frame == 0:
            self.frame = 1
        else :
            self.frame = 0
        
    
    def Clear(self):        
        for i in range (len(self.buf)):
            self.buf[i] = 0
        
    def __Clamp(self, value: int, min: int, max: int):
        if (value < min):
            return min
        if (value > max):
            return max
        
        return value


    def SetPixel(self, col: int,  row: int, brightness = 128)   :
        if (col < 0 or row < 0 or col >= ScrollBitController.COLS or row >= ScrollBitController.ROWS):
            raise Exception("Invalid agruments")
           
        if (self.UpSideDown) :
            col = (ScrollBitController.COLS - 1) - col
            row = (ScrollBitController.ROWS - 1) - row
        
        index = self.__PixelAddr(col, row);
        self.buf[index] = self.__Clamp(brightness, 0, 255)

    def __PixelAddr(self, col: int, row: int) :
            row = 7 - (row + 1)

            if (col > 8) :
                col = col - 8
                row = 6 - (row + 8)
            
            else:
                col = 8 - col
            
            return ((col * 16) + row)
        
    


    def DrawString(self, text, color, x, y, hScale = 1, vScale=1):
        originalX = x
        for i in range(len(text)):
            if ord(text[i]) >=32 :
                self.__DrawCharacter(ord(text[i]), color, x, y, hScale, vScale)
                x += 6*hScale
            else:
                if text[i] == '\n':
                    y += 9 * vScale
                    x = originalX
                else:
                    if text[i] == '\r':
                        x = originalX


    def __DrawCharacter(self, character, color, x, y, hScale = 1, vScale = 1):
        index = 5 * (character - 32)
        
        if (hScale != 1 or vScale != 1):
            for horizontalFontSize in range(5):
                for hs in range(hScale):
                    for verticleFontSize in range(8):
                        for vs in range (vScale):                                                
                            if (mono8x5[index + horizontalFontSize] & (1 << verticleFontSize)) != 0:
                                self.SetPixel(x + (horizontalFontSize * hScale) + hs, y + (verticleFontSize * vScale) + vs, color)
        else:
            for horizontalFontSize in range(5):
                sx = x + horizontalFontSize
                fontRow = mono8x5[index + horizontalFontSize]
                for verticleFontSize in range(8):
                    if ((fontRow & (1 << verticleFontSize)) != 0):
                        self.SetPixel(sx, y + verticleFontSize, color)

mono8x5 = [
    0x00, 0x00, 0x00, 0x00, 0x00, #  Space	0x20 */
    0x00, 0x00, 0x4f, 0x00, 0x00, #  ! */
    0x00, 0x07, 0x00, 0x07, 0x00, #  " */
    0x14, 0x7f, 0x14, 0x7f, 0x14, #  # */
    0x24, 0x2a, 0x7f, 0x2a, 0x12, #  $ */
    0x23, 0x13, 0x08, 0x64, 0x62, #  % */
    0x36, 0x49, 0x55, 0x22, 0x20, #  & */
    0x00, 0x05, 0x03, 0x00, 0x00, #  ' */
    0x00, 0x1c, 0x22, 0x41, 0x00, #  ( */
    0x00, 0x41, 0x22, 0x1c, 0x00, #  ) */
    0x14, 0x08, 0x3e, 0x08, 0x14, #  // */
    0x08, 0x08, 0x3e, 0x08, 0x08, #  + */
    0x50, 0x30, 0x00, 0x00, 0x00, #  , */
    0x08, 0x08, 0x08, 0x08, 0x08, #  - */
    0x00, 0x60, 0x60, 0x00, 0x00, #  . */
    0x20, 0x10, 0x08, 0x04, 0x02, #  / */
    0x3e, 0x51, 0x49, 0x45, 0x3e, #  0		0x30 */
    0x00, 0x42, 0x7f, 0x40, 0x00, #  1 */
    0x42, 0x61, 0x51, 0x49, 0x46, #  2 */
    0x21, 0x41, 0x45, 0x4b, 0x31, #  3 */
    0x18, 0x14, 0x12, 0x7f, 0x10, #  4 */
    0x27, 0x45, 0x45, 0x45, 0x39, #  5 */
    0x3c, 0x4a, 0x49, 0x49, 0x30, #  6 */
    0x01, 0x71, 0x09, 0x05, 0x03, #  7 */
    0x36, 0x49, 0x49, 0x49, 0x36, #  8 */
    0x06, 0x49, 0x49, 0x29, 0x1e, #  9 */
    0x00, 0x36, 0x36, 0x00, 0x00, #  : */
    0x00, 0x56, 0x36, 0x00, 0x00, #  ; */
    0x08, 0x14, 0x22, 0x41, 0x00, #  < */
    0x14, 0x14, 0x14, 0x14, 0x14, #  = */
    0x00, 0x41, 0x22, 0x14, 0x08, #  > */
    0x02, 0x01, 0x51, 0x09, 0x06, #  ? */
    0x3e, 0x41, 0x5d, 0x55, 0x1e, #  @		0x40 */
    0x7e, 0x11, 0x11, 0x11, 0x7e, #  A */
    0x7f, 0x49, 0x49, 0x49, 0x36, #  B */
    0x3e, 0x41, 0x41, 0x41, 0x22, #  C */
    0x7f, 0x41, 0x41, 0x22, 0x1c, #  D */
    0x7f, 0x49, 0x49, 0x49, 0x41, #  E */
    0x7f, 0x09, 0x09, 0x09, 0x01, #  F */
    0x3e, 0x41, 0x49, 0x49, 0x7a, #  G */
    0x7f, 0x08, 0x08, 0x08, 0x7f, #  H */
    0x00, 0x41, 0x7f, 0x41, 0x00, #  I */
    0x20, 0x40, 0x41, 0x3f, 0x01, #  J */
    0x7f, 0x08, 0x14, 0x22, 0x41, #  K */
    0x7f, 0x40, 0x40, 0x40, 0x40, #  L */
    0x7f, 0x02, 0x0c, 0x02, 0x7f, #  M */
    0x7f, 0x04, 0x08, 0x10, 0x7f, #  N */
    0x3e, 0x41, 0x41, 0x41, 0x3e, #  O */
    0x7f, 0x09, 0x09, 0x09, 0x06, #  P		0x50 */
    0x3e, 0x41, 0x51, 0x21, 0x5e, #  Q */
    0x7f, 0x09, 0x19, 0x29, 0x46, #  R */
    0x26, 0x49, 0x49, 0x49, 0x32, #  S */
    0x01, 0x01, 0x7f, 0x01, 0x01, #  T */
    0x3f, 0x40, 0x40, 0x40, 0x3f, #  U */
    0x1f, 0x20, 0x40, 0x20, 0x1f, #  V */
    0x3f, 0x40, 0x38, 0x40, 0x3f, #  W */
    0x63, 0x14, 0x08, 0x14, 0x63, #  X */
    0x07, 0x08, 0x70, 0x08, 0x07, #  Y */
    0x61, 0x51, 0x49, 0x45, 0x43, #  Z */
    0x00, 0x7f, 0x41, 0x41, 0x00, #  [ */
    0x02, 0x04, 0x08, 0x10, 0x20, #  \ */
    0x00, 0x41, 0x41, 0x7f, 0x00, #  ] */
    0x04, 0x02, 0x01, 0x02, 0x04, #  ^ */
    0x40, 0x40, 0x40, 0x40, 0x40, #  _ */
    0x00, 0x00, 0x03, 0x05, 0x00, #  `		0x60 */
    0x20, 0x54, 0x54, 0x54, 0x78, #  a */
    0x7F, 0x44, 0x44, 0x44, 0x38, #  b */
    0x38, 0x44, 0x44, 0x44, 0x44, #  c */
    0x38, 0x44, 0x44, 0x44, 0x7f, #  d */
    0x38, 0x54, 0x54, 0x54, 0x18, #  e */
    0x04, 0x04, 0x7e, 0x05, 0x05, #  f */
    0x08, 0x54, 0x54, 0x54, 0x3c, #  g */
    0x7f, 0x08, 0x04, 0x04, 0x78, #  h */
    0x00, 0x44, 0x7d, 0x40, 0x00, #  i */
    0x20, 0x40, 0x44, 0x3d, 0x00, #  j */
    0x7f, 0x10, 0x28, 0x44, 0x00, #  k */
    0x00, 0x41, 0x7f, 0x40, 0x00, #  l */
    0x7c, 0x04, 0x7c, 0x04, 0x78, #  m */
    0x7c, 0x08, 0x04, 0x04, 0x78, #  n */
    0x38, 0x44, 0x44, 0x44, 0x38, #  o */
    0x7c, 0x14, 0x14, 0x14, 0x08, #  p		0x70 */
    0x08, 0x14, 0x14, 0x14, 0x7c, #  q */
    0x7c, 0x08, 0x04, 0x04, 0x08, #  r */
    0x48, 0x54, 0x54, 0x54, 0x24, #  s */
    0x04, 0x04, 0x3f, 0x44, 0x44, #  t */
    0x3c, 0x40, 0x40, 0x20, 0x7c, #  u */
    0x1c, 0x20, 0x40, 0x20, 0x1c, #  v */
    0x3c, 0x40, 0x30, 0x40, 0x3c, #  w */
    0x44, 0x28, 0x10, 0x28, 0x44, #  x */
    0x0c, 0x50, 0x50, 0x50, 0x3c, #  y */
    0x44, 0x64, 0x54, 0x4c, 0x44, #  z */
    0x08, 0x36, 0x41, 0x41, 0x00, #  { */
    0x00, 0x00, 0x77, 0x00, 0x00, #  | */
    0x00, 0x41, 0x41, 0x36, 0x08, #  } */
    0x08, 0x08, 0x2a, 0x1c, 0x08  #  ~ */
    ]






