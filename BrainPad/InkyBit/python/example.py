from DUE.DUEController import DUEController
from InkyBit import InkyBitController
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

screen = InkyBitController(dueController)

screen.Clear()
screen.DrawString("DUE - InkyBit", 1, 5,5,3,3)


screen.Show()




