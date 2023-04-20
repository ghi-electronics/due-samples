using GHIElectronics.DUE;
using MMC5603NJ;

var port = DUEController.GetConnectionPort();
var dueController = new DUEController(port);

var sensor = new MMC56X3Controller(dueController);

while (true) {
    sensor.UpdateXYZ();
    Console.WriteLine(string.Format("value: X= {0}, Y= {1}, Z= {2}, temperature {3}", sensor.X, sensor.Y, sensor.Z, sensor.Temperature.ToString("F2")));

    Thread.Sleep(1000);
}
