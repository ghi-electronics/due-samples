from DUELink.DUELinkController import DUELinkController
import time

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

class ST7735CommandId:
        #System
        NOP = 0x00
        SWRESET = 0x01
        RDDID = 0x04
        RDDST = 0x09
        RDDPM = 0x0A
        RDDMADCTL = 0x0B
        RDDCOLMOD = 0x0C
        RDDIM = 0x0D
        RDDSM = 0x0E
        SLPIN = 0x10
        SLPOUT = 0x11
        PTLON = 0x12
        NORON = 0x13
        INVOFF = 0x20
        INVON = 0x21
        GAMSET = 0x26
        DISPOFF = 0x28
        DISPON = 0x29
        CASET = 0x2A
        RASET = 0x2B
        RAMWR = 0x2C
        RAMRD = 0x2E
        PTLAR = 0x30
        TEOFF = 0x34
        TEON = 0x35
        MADCTL = 0x36
        IDMOFF = 0x38
        IDMON = 0x39
        COLMOD = 0x3A
        RDID1 = 0xDA
        RDID2 = 0xDB
        RDID3 = 0xDC

        #Panel
        FRMCTR1 = 0xB1
        FRMCTR2 = 0xB2
        FRMCTR3 = 0xB3
        INVCTR = 0xB4
        DISSET5 = 0xB6
        PWCTR1 = 0xC0
        PWCTR2 = 0xC1
        PWCTR3 = 0xC2
        PWCTR4 = 0xC3
        PWCTR5 = 0xC4
        VMCTR1 = 0xC5
        VMOFCTR = 0xC7
        WRID2 = 0xD1
        WRID3 = 0xD2
        NVCTR1 = 0xD9
        NVCTR2 = 0xDE
        NVCTR3 = 0xDF
        GAMCTRP1 = 0xE0
        GAMCTRN1 = 0xE1

class ST7735Controller:
    internalBuffer = [0]*(160*128)
    buffer1 = bytearray(1)
    buffer4 = bytearray(4)
    Width = 160
    Height = 128

    

    def __init__(self, dueController: DUELinkController, chipselectPin: int, controlPin: int, resetPin: int, backlightPin: int) -> None:
        self.dueController = dueController
        self.resetPin = resetPin
        self.backlightPin = backlightPin
        self.controlPin = controlPin
        self.chipselectPin = chipselectPin
        self.__Reset()
        self.__Initialize()
        self.SetDataAccessControl(True, False, True, False)
        self.SetDrawWindow(1, 2, self.Width - 1, self.Height - 1)
        self.Enable()

    def __Reset(self):
        if self.backlightPin != -1:
            self.dueController.Digital.Write(self.backlightPin, True)

        if (self.resetPin != -1) :
            self.dueController.Digital.Write(self.resetPin, False)
            time.sleep(50/1000)

            self.dueController.Digital.Write(self.resetPin, True)
            time.sleep(50/200)
          
    def __Initialize(self):
        self.__SendCommand(ST7735CommandId.SWRESET)
        time.sleep(50/120)

        self.__SendCommand(ST7735CommandId.SLPOUT)
        time.sleep(50/120)

        self.__SendCommand(ST7735CommandId.FRMCTR1)
        self.__SendByte(0x01)
        self.__SendByte(0x2C)
        self.__SendByte(0x2D)

        self.__SendCommand(ST7735CommandId.FRMCTR2)
        self.__SendByte(0x01)
        self.__SendByte(0x2C)
        self.__SendByte(0x2D)

        self.__SendCommand(ST7735CommandId.FRMCTR3)
        self.__SendByte(0x01)
        self.__SendByte(0x2C)
        self.__SendByte(0x2D)
        self.__SendByte(0x01)
        self.__SendByte(0x2C)
        self.__SendByte(0x2D)

        self.__SendCommand(ST7735CommandId.INVCTR)
        self.__SendByte(0x07)

        self.__SendCommand(ST7735CommandId.PWCTR1)
        self.__SendByte(0xA2)
        self.__SendByte(0x02)
        self.__SendByte(0x84)

        self.__SendCommand(ST7735CommandId.PWCTR2)
        self.__SendByte(0xC5)

        self.__SendCommand(ST7735CommandId.PWCTR3)
        self.__SendByte(0x0A)
        self.__SendByte(0x00)

        self.__SendCommand(ST7735CommandId.PWCTR4)
        self.__SendByte(0x8A)
        self.__SendByte(0x2A)

        self.__SendCommand(ST7735CommandId.PWCTR5)
        self.__SendByte(0x8A)
        self.__SendByte(0xEE)

        self.__SendCommand(ST7735CommandId.VMCTR1)
        self.__SendByte(0x0E)

        self.__SendCommand(ST7735CommandId.GAMCTRP1)
        self.__SendByte(0x0F)
        self.__SendByte(0x1A)
        self.__SendByte(0x0F)
        self.__SendByte(0x18)
        self.__SendByte(0x2F)
        self.__SendByte(0x28)
        self.__SendByte(0x20)
        self.__SendByte(0x22)
        self.__SendByte(0x1F)
        self.__SendByte(0x1B)
        self.__SendByte(0x23)
        self.__SendByte(0x37)
        self.__SendByte(0x00)
        self.__SendByte(0x07)
        self.__SendByte(0x02)
        self.__SendByte(0x10)

        self.__SendCommand(ST7735CommandId.GAMCTRN1)
        self.__SendByte(0x0F)
        self.__SendByte(0x1B)
        self.__SendByte(0x0F)
        self.__SendByte(0x17)
        self.__SendByte(0x33)
        self.__SendByte(0x2C)
        self.__SendByte(0x29)
        self.__SendByte(0x2E)
        self.__SendByte(0x30)
        self.__SendByte(0x30)
        self.__SendByte(0x39)
        self.__SendByte(0x3F)
        self.__SendByte(0x00)
        self.__SendByte(0x07)
        self.__SendByte(0x03)
        self.__SendByte(0x10)

        # Force to 565 color format
        self.__SendCommand(ST7735CommandId.COLMOD)
        self.__SendByte(0x05)


    def Enable(self):
        self.__SendCommand(ST7735CommandId.DISPON)

    def Disable(self):
        self.__SendCommand(ST7735CommandId.DISPOFF)

    def __SendCommand(self, cmd: int):
        self.buffer1[0] = cmd
        self.dueController.Digital.Write(self.controlPin, False)
        self.dueController.Spi.Write(self.buffer1, 0, len(self.buffer1), self.chipselectPin)

    def __SendByte(self, data: int):
        self.buffer1[0] = data
        self.dueController.Digital.Write(self.controlPin, True)
        self.dueController.Spi.Write(self.buffer1, 0, len(self.buffer1),self.chipselectPin)

    def __SendBytes(self, data: bytearray):
        self.dueController.Digital.Write(self.controlPin, True)
        self.dueController.Spi.Write(data, 0, len(data),self.chipselectPin)

    def SetDataAccessControl(self, swapRowColumn: bool, invertRow: bool, invertColumn: bool, useBgrPanel: bool) :
            val = 0

            if (useBgrPanel):
                val |= 0b0000_1000

            if (swapRowColumn):
                val |= 0b0010_0000

            if (invertColumn):
                val |= 0b0100_0000

            if (invertRow):
                val |= 0b1000_0000

            self.__SendCommand(ST7735CommandId.MADCTL)
            self.__SendByte(val)

    def SetDrawWindow(self, x: int, y: int, width: int, height: int) :

            self.buffer4[1] = x
            self.buffer4[3] = (x + width)
            self.__SendCommand(ST7735CommandId.CASET)
            self.__SendBytes(self.buffer4)

            self.buffer4[1] =y
            self.buffer4[3] = (y + height)
            self.__SendCommand(ST7735CommandId.RASET)
            self.__SendBytes(self.buffer4)
             
    def SendDrawCommand(self) :
            self.__SendCommand(ST7735CommandId.RAMWR)
            self.dueController.Digital.Write(self.controlPin, True)
        
    def __SwapEndianness(self, data: bytearray) :
            for i in range (0, len(data),2):
                t = data[i]
                data[i] = (data[i + 1])
                data[i + 1] = t


    def DrawBuffer(self, color, offset: int, length: int, _4bpp = False) :
        if color == 0 or (offset+ length) > len(color):
            raise ("Argument out of range exception")
        
        buffer: bytearray

        if (_4bpp) :
            buffer = bytearray(int(length / 2))
            

            for i in range(len(buffer)):
              

                buffer[i] = (((color[(i + offset) * 2] << 4) | color[(i + offset) * 2 + 1]))
            
        else :
            buffer = bytearray(int(length *2 ))
            i = 0
            for y in range(self.Height):
                for x in range(self.Width):
                    index = (y * self.Width + x) * 2
                    clr = color[i + offset]

                    buffer[index + 0] =(((clr & 0b0000_0000_0000_0000_0001_1100_0000_0000) >> 5) | ((clr & 0b0000_0000_0000_0000_0000_0000_1111_1000) >> 3))
                    buffer[index + 1] =(((clr & 0b0000_0000_1111_1000_0000_0000_0000_0000) >> 16) | ((clr & 0b0000_0000_0000_0000_1110_0000_0000_0000) >> 13))
                    i += 1

                
            
        
    
        self.SendDrawCommand()

        if _4bpp:
            self.dueController.Spi.Write4bpp(buffer, offset, length, self.chipselectPin)
            return
        
        self.__SwapEndianness(buffer)
        self.dueController.Spi.Write(buffer, 0, len(buffer), self.chipselectPin)
        self.__SwapEndianness(buffer)


    def Show(self) :                                 
        self.DrawBuffer(self.internalBuffer, 0, len(self.internalBuffer))  

    def Clear(self):
        for i in range(len(self.internalBuffer)):
            self.internalBuffer[i] = 0
 

    def SetPixel(self, x: int, y: int, color: int):
        if (x < 0 or y < 0 or  x >= self.Width or y >= self.Height):
            return
        
        self.internalBuffer[y * self.Width + x] = color
        
        #index = (y * self.Width + x) * 2
        #clr = color

        #self.internalBuffer[index + 0] = (((clr & 0b0000_0000_0000_0000_0001_1100_0000_0000) >> 5) | ((clr & 0b0000_0000_0000_0000_0000_0000_1111_1000) >> 3))
        #self.internalBuffer[index + 1] = (((clr & 0b0000_0000_1111_1000_0000_0000_0000_0000) >> 16) | ((clr & 0b0000_0000_0000_0000_1110_0000_0000_0000) >> 13))

    

    def DrawCharacter(self, character, color, x, y, hScale = 1, vScale = 1):
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
    
    def DrawString(self, text, color, x, y, hScale = 1, vScale=1):
        originalX = x
        for i in range(len(text)):
            if ord(text[i]) >=32 :
                self.DrawCharacter(ord(text[i]), color, x, y, hScale, vScale)
                x += 6*hScale
            else:
                if text[i] == '\n':
                    y += 9 * vScale
                    x = originalX
                else:
                    if text[i] == '\r':
                        x = originalX






