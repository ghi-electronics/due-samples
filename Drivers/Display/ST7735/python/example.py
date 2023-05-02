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

# 4bpp example - show Pallete
data = [0] * (160*128)
color = 0

for y in range (128):
    for x in range (160):
        data[y * 160 + x] = color
    
    if (((y % 8) == 0) and (y != 0)):
        color += 1

screen.DrawBuffer(data, 0, len(data) ,True)
