from DUELink.DUELinkController import DUELinkController
from hdc1000 import HDC1000Controller
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

sensor = HDC1000Controller(dueController)


while True:
    print(f"Temperature = {sensor.Temperature}, Humidity = {sensor.Humidity}")
    time.sleep(1)
