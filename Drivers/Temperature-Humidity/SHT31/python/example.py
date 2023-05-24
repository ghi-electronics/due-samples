from DUELink.DUELinkController import DUELinkController
from sht31 import SHT31Controller
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

sensor = SHT31Controller(dueController)


while True:
    print(f"Temperature = {sensor.Temperature}, Humidity = {sensor.Humidity}")
    time.sleep(1)
