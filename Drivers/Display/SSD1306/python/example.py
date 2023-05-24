from DUELink.DUELinkController import DUELinkController
from SSD1306 import SSD1306Controller
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

screen = SSD1306Controller(dueController)

screen.Clear()

screen.DrawString("DUE - SSD1306", 1, 5, 5)

screen.Show()

time.sleep(1)

#Draw half screen black and half screen color by buffer
data = [0] * (128*64)
for y in range(64):
    for x in range (128):
        if y > 32 :
            data[y * 128 + x] = 0
        else:
            data[y * 128 + x] = 0xFFFFFFFF


screen.DrawBuffer(data, 0, len(data))