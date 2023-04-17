from DUE.DUEController import DUEController
from ST7735 import ST7735Controller


availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

chipselect = 16; # DUE pin
controlPin = 12; # DUE pin
resetPin = 8; # DUE pin
backlightPin = 1; # DUE pin

screen = ST7735Controller(dueController, chipselect, controlPin, resetPin, backlightPin)

screen.Clear()

screen.DrawString("DUE ST7735", 0x00FF00, 5, 5)

screen.Show()

# 4bpp example
#
#data = bytearray(int(160 * 128 / 2))
#color = 0

#for i in range (0, len(data), 640):
#    for c in range (i, i + 640):
#        data[c] = color << 4 | color
#    color = color + 1

#screen.ShowData(data, 0, len(data) ,True)
