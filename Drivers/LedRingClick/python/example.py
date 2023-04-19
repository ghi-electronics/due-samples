from DUE.DUEController import DUEController
from ringclick import RingClickController
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

resetPin = 6
latchPin = 17
dataPin = 16
clockPin = 18

led = RingClickController(dueController, resetPin, latchPin, dataPin, clockPin)

while True:

    for i in range (32):
        led.Set(i, True)
        led.Show()

    led.Clear()
