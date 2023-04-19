from DUE.DUEController import DUEController
from gyro import GyroscopeController
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

gyro = GyroscopeController(dueController)


while True:
    if (gyro.AccelerationAvailable()):
        print(f"X = {gyro.X}, Y = {gyro.Y},  Z = {gyro.Z}")
    time.sleep(1)




