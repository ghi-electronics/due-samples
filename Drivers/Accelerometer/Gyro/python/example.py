from DUELink.DUELinkController import DUELinkController
from gyro import GyroscopeController
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

gyro = GyroscopeController(dueController)


while True:
    if (gyro.AccelerationAvailable()):
        print(f"X = {gyro.X}, Y = {gyro.Y},  Z = {gyro.Z}")
    time.sleep(1)




