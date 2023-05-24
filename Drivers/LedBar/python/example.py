from DUELink.DUELinkController import DUELinkController
from ledbar import LedBarController
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

pinClock = 4
pinDio = 5
led = LedBarController(dueController,pinClock, pinDio)

while True:
    led.ReverseShow = not led.ReverseShow
    for i in range (led.LedNum):
        led.SetLed(i, 255)
        led.Show()

    for i in range (led.LedNum):
        led.SetLed(i, 0)
        led.Show()
