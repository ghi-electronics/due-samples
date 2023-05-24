from DUELink.DUELinkController import DUELinkController
from InkyBit import InkyBitController
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

screen = InkyBitController(dueController)

screen.Clear()
screen.DrawString("DUE - InkyBit", 1, 5,5,3,3)


screen.Show()




