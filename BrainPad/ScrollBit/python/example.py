from DUE.DUEController import DUEController
from scrollbit import ScrollBitController
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

screen = ScrollBitController(dueController)

screen.Clear()
screen.DrawString("DUE", 128, 0, 0)
screen.Show()




