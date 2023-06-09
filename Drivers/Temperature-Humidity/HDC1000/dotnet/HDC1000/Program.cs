
using GHIElectronics.DUELink;
using HDC1000;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var hdc1000 = new HDC1000Controller(dueController);

while (true) {
    Console.WriteLine(string.Format("Temperature = {0}, Humidity = {1}", hdc1000.Temperature.ToString("F2"), hdc1000.Humidity.ToString("F2")));

    Thread.Sleep(1000);
}



