from DUELink.DUELinkController import DUELinkController
from scrollbit import ScrollBitController
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

screen = ScrollBitController(dueController)

screen.Clear()
screen.DrawString("DUE", 128, 0, 0)
screen.Show()




