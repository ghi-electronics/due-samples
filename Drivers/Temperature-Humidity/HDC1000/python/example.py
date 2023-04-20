from DUE.DUEController import DUEController
from hdc1000 import HDC1000Controller
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

sensor = HDC1000Controller(dueController)


while True:
    print(f"Temperature = {sensor.Temperature}, Humidity = {sensor.Humidity}")
    time.sleep(1)
