from DUE.DUEController import DUEController
from accelg248 import AccelG248Controller
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

sensor = AccelG248Controller(dueController)


while True:
    print(f"X = {sensor.X}, Y = {sensor.Y},  Z = {sensor.Z}")
    time.sleep(1)




