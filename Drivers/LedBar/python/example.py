from DUE.DUEController import DUEController
from ledbar import LedBarController
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

pinClock = 4
pinDio = 5
led = LedBarController(dueController,pinClock, pinDio)

while True:
    for i in range (led.LedNum):
        led.SetLed(i, 255)

    for i in range (led.LedNum):
        led.SetLed(i, 0)
