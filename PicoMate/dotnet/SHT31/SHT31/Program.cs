using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var sht32 = new SHT31.SHT31Controller(dueController, 0x44);

while (true) {
    Console.WriteLine(string.Format("Temperature: {0}, Humidity: {1}", sht32.Temperature, sht32.Humidity));

    Thread.Sleep(1000);
}
