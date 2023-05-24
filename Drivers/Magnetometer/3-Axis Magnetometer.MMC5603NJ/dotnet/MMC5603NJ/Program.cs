using GHIElectronics.DUELink;
using MMC5603NJ;

var port = DUELinkController.GetConnectionPort();
var dueController = new DUELinkController(port);

var sensor = new MMC56X3Controller(dueController);

while (true) {
    //Console.WriteLine(string.Format("value: X= {0}, Y= {1}, Z= {2}, temperature {3}", sensor.X, sensor.Y, sensor.Z, sensor.Temperature.ToString("F2")));
    Console.WriteLine(string.Format("value: X= {0}", sensor.X));

    Thread.Sleep(1000);
}
