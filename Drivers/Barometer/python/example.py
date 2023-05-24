from DUELink.DUELinkController import DUELinkController
from bmp280 import BMP280Controller
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)


bmp280 = BMP280Controller(dueController)

while True:
    print(f"Pressure = {bmp280.GetPressure()/100} Hpa, Temperature = {bmp280.GetTemperature()} C, Altitude: {bmp280.CalculateAltitude()}m")

    time.sleep(1)


