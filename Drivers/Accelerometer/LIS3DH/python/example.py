from DUE.DUEController import DUEController
from lis3dh import LIS3DHController
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

lis3dh = LIS3DHController(dueController)


while True:
    print(f"X = {lis3dh.X}, Y = {lis3dh.Y},  Z = {lis3dh.Z}")
    time.sleep(1)




