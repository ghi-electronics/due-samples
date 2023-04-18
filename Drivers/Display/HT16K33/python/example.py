from DUE.DUEController import DUEController
from HT16K33 import HT16K33Controller
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)



screen = HT16K33Controller(dueController)

screen.Brightness = 0

counter = 0
while True:
    screen.Clear()
    screen.DrawCharacter(str(counter % 10), 1, 0, 0)
    screen.Show()

    counter = counter + 1
    screen.Rotation = counter % 4
    time.sleep(1)




