from DUELink.DUELinkController import DUELinkController
from accelg248 import AccelG248Controller
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

sensor = AccelG248Controller(dueController)

while True:
    print(f"X = {sensor.X}, Y = {sensor.Y},  Z = {sensor.Z}")
    time.sleep(1)




