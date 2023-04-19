using BMP280;
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var bmp280 = new BMP280Controller(dueController);

while (true) {
    Console.WriteLine(string.Format("Pressure = {0} Hpa, Temperature = {1} C, Altitude: {2}m", (bmp280.GetPressure()/100).ToString("0.00"), bmp280.GetTemperature().ToString("0.00"), bmp280.CalculateAltitude().ToString("0.00")));

    Thread.Sleep(500);
}
