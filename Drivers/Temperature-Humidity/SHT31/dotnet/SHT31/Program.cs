using GHIElectronics.DUELink;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var sht32 = new SHT31.SHT31Controller(dueController);

while (true) {
    Console.WriteLine(string.Format("Temperature: {0}, Humidity: {1}", sht32.Temperature, sht32.Humidity));

    Thread.Sleep(1000);
}
