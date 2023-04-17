from DUE.DUEController import DUEController
from SSD1306 import SSD1306Controller


availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

screen = SSD1306Controller(dueController)

screen.Clear()

screen.DrawString("DUE - SSD1306", 1, 5, 5)

screen.Show()

