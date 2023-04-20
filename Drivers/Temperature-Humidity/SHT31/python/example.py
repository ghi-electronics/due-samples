from DUE.DUEController import DUEController
from sht31 import SHT31Controller
import time

availablePort = DUEController.GetConnectionPort()

dueController = DUEController(availablePort)

sensor = SHT31Controller(dueController)


while True:
    print(f"Temperature = {sensor.Temperature}, Humidity = {sensor.Humidity}")
    time.sleep(1)
