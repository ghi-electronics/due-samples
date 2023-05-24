from DUELink.DUELinkController import DUELinkController
from mmc56x3 import MMC56x3Controller
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

sensor = MMC56x3Controller(dueController)

while True:
    print(f"X = {sensor.X}, Y = {sensor.Y},  Z = {sensor.Z}, temperature = {sensor.Temperature}")

    time.sleep(1)




