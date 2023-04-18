from DUE.DUEController import DUEController
from TM1637 import TM1637Controller
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

pinClk = 5
pinDio = 4
sevendigits = TM1637Controller(dueController, pinClk, pinDio)

sevendigits.Brightness = 7


counter = 1000

while True:
    sevendigits.ShowNumberDec(counter, True)
    time.sleep(0.5)
    sevendigits.ShowNumberDec(counter, False)
    time.sleep(0.5)

    counter += 1



