from DUE.DUEController import DUEController
from apa102 import APA102Controller
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

led = APA102Controller(dueController)

colors = [ 0xFFFFFF, 0xFF0000, 0x00FF00, 0x0000FF ]
colorIndex = 0

while True:
    red = (colors[colorIndex % 4] >> 16) & 0xFF
    green = (colors[colorIndex % 4] >> 8) & 0xFF
    blue = (colors[colorIndex % 4] >> 0) & 0xFF

    for i in range (led.LedCount):
        if (i ==0):
            led.Set(led.LedCount-1, 0, 0, 0)
        
        else:
            led.Set(i - 1, 0, 0, 0)        

        time.sleep(10/1000) # delay 10ms every set
        
        led.Set(i, red,green,blue)

        time.sleep(100/1000)
    
    colorIndex +=1
