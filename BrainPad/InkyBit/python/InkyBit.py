from DUE.DUEController import DUEController
from DUE.Digital import Input
import array
import time
import numpy

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



class InkyBitController:    


    DRIVER_CONTROL = 0x01
    GATE_VOLTAGE = 0x03
    SOURCE_VOLTAGE = 0x04
    DISPLAY_CONTROL = 0x07
    NON_OVERLAP = 0x0B
    BOOSTER_SOFT_START = 0x0C
    GATE_SCAN_START = 0x0F
    DEEP_SLEEP = 0x10
    DATA_MODE = 0x11
    SW_RESET = 0x12
    TEMP_WRITE = 0x1A
    TEMP_READ = 0x1B
    TEMP_CONTROL = 0x1C
    TEMP_LOAD = 0x1D
    MASTER_ACTIVATE = 0x20
    DISP_CTRL1 = 0x21
    DISP_CTRL2 = 0x22
    WRITE_RAM = 0x24
    WRITE_ALTRAM = 0x26
    READ_RAM = 0x25
    VCOM_SENSE = 0x28
    VCOM_DURATION = 0x29
    WRITE_VCOM = 0x2C
    READ_OTP = 0x2D
    WRITE_LUT = 0x32
    WRITE_DUMMY = 0x3A
    WRITE_GATELINE = 0x3B
    WRITE_BORDER = 0x3C
    SET_RAMXPOS = 0x44
    SET_RAMYPOS = 0x45
    SET_RAMXCOUNT = 0x4E
    SET_RAMYCOUNT = 0x4F

    PIN_DC = 12 #  uBit.io.P12   // MICROBIT_PIN_P12
    PIN_CS = 8 #  uBit.io.P8    // MICROBIT_PIN_P8
    PIN_RESET = 2 #  uBit.io.P2 // MICROBIT_PIN_P2
    PIN_BUSY = 16 #  uBit.io.P16 // MICROBIT_PIN_P16


    CS_ACTIVE = False
    CS_INACTIVE = True
    DC_DATA = True
    DC_COMMAND = False




    COLS = 136
    ROWS = 250
    OFFSET_X = 0
    OFFSET_Y = 6

    def __init__(self, dueController: DUEController ) -> None:
        self.dueController = dueController
        self.dueController.Digital.Write(InkyBitController.PIN_CS, InkyBitController.CS_INACTIVE)

        self.__Width = 250
        self.__Height = 122
            
        self.buf_r = bytearray(int(InkyBitController.COLS / 8) * InkyBitController.ROWS) 
        self.buf_b = bytearray(int(InkyBitController.COLS / 8) * InkyBitController.ROWS )
        self.luts = [
            0x02, 0x02, 0x01, 0x11, 0x12, 0x12, 0x22, 0x22, 0x66, 0x69,
            0x69, 0x59, 0x58, 0x99, 0x99, 0x88, 0x00, 0x00, 0x00, 0x00,
            0xF8, 0xB4, 0x13, 0x51, 0x35, 0x51, 0x51, 0x19, 0x01, 0x00
        ]

        self.Clear()

    def BusyWait(self):
        while self.dueController.Digital.Read(InkyBitController.PIN_BUSY, Input.PULL_UP) == True:
            time.sleep(50/1000)
    
    def Clear(self):        
        for i in range (len(self.buf_r)):
            self.buf_r[i] = 0

        for i in range (len(self.buf_b)):
            self.buf_b[i] = 0xFF        

    def SetPixel(self, x: int, y: int, color: int)   :
        if (x >= self.__Width):
            return
        if (y >= self.__Height):
            return
        y += InkyBitController.OFFSET_Y
        y = InkyBitController.COLS - 1 - y
        
        shift = 7 - int(y % 8)
        y = int(y / 8)
        offset = (x * int(InkyBitController.COLS / 8)) + y

        byte_b = self.buf_b[offset] | (0b1 << shift)
        byte_r = self.buf_r[offset] & (~(0b1 << shift))

        if (color == 2):
            byte_r |= 0b1 << shift
        
        if (color == 1):
            byte_b &= ~(0b1 << shift)
        

        self.buf_b[offset] = byte_b & 0xFF
        self.buf_r[offset] = byte_r & 0xFF

    def SendCommand(self, command: int):
        self.SendCommands(command, 0, 0)

    def SendCommands(self, command: int, data: bytearray, length: int):
        self.dueController.Digital.Write(InkyBitController.PIN_CS,InkyBitController.CS_ACTIVE)
        self.dueController.Digital.Write(InkyBitController.PIN_DC, InkyBitController.DC_COMMAND)

        temp = [ command]

        self.dueController.Spi.Write(temp, 0, 1)
        self.dueController.Digital.Write(InkyBitController.PIN_DC, InkyBitController.DC_DATA)

        if (data != 0 and length > 0) :
            self.dueController.Spi.Write(data,0, length)

        self.dueController.Digital.Write(InkyBitController.PIN_CS, InkyBitController.CS_INACTIVE)

    def SendData(self, data: bytes)    :
        self.dueController.Digital.Write(InkyBitController.PIN_CS, InkyBitController.CS_ACTIVE)

        self.dueController.Digital.Write(InkyBitController.PIN_DC, InkyBitController.DC_DATA)
        self.dueController.Spi.Write(data, 0, len(data))

        self.dueController.Digital.Write(InkyBitController.PIN_CS, InkyBitController.CS_ACTIVE)
        
    def Show(self):
        self.dueController.Digital.Write(InkyBitController.PIN_RESET, False)

        time.sleep(100/1000)            
        self.dueController.Digital.Write(InkyBitController.PIN_RESET, True)

        time.sleep(100/1000)

        self.SendCommand(0x12)
        time.sleep(500/1000)
        self.BusyWait()

        data = [InkyBitController.ROWS - 1, (InkyBitController.ROWS - 1) >> 8, 0x00]
        self.SendCommands(InkyBitController.DRIVER_CONTROL, data, 3)

        data = [0x1B]
        self.SendCommands(InkyBitController.WRITE_DUMMY, data, 1)

        data = [0x0B]
        self.SendCommands(InkyBitController.WRITE_GATELINE, data, 1)

        data = [0x03]
        self.SendCommands(InkyBitController.DATA_MODE, data, 1)

        data = [0x00, int(InkyBitController.COLS / 8) - 1]
        self.SendCommands(InkyBitController.SET_RAMXPOS, data, 2)

        data = [0x00, 0x00, (InkyBitController.ROWS - 1) & 0xFF, (InkyBitController.ROWS - 1) >> 8]
        self.SendCommands(InkyBitController.SET_RAMYPOS, data, 4)

        data = [0x70]
        self.SendCommands(InkyBitController.WRITE_VCOM, data, 1)

        
        self.SendCommands(InkyBitController.WRITE_LUT, self.luts, len(self.luts))

        data = [0x00]
        self.SendCommands(InkyBitController.SET_RAMXCOUNT, data, 1)

        data = [0x00, 0x00]
        self.SendCommands(InkyBitController.SET_RAMYCOUNT, data, 2)

        self.SendCommand(InkyBitController.WRITE_RAM)
        self.SendData(self.buf_b)
        self.SendCommand(InkyBitController.WRITE_ALTRAM)
        self.SendData(self.buf_r)

        self.BusyWait()
        self.SendCommand(InkyBitController.MASTER_ACTIVATE)


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






