using System.Security.Cryptography.X509Certificates;
using GHIElectronics.DUELink;
using Gyroscope;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var gyroscope = new GyroscopeController(dueController);

while (true) {
    if (gyroscope.AccelerationAvailable())
        Console.WriteLine(string.Format("Accel: X = {0}, Y = {1}, Z = {2}", gyroscope.X, gyroscope.Y, gyroscope.Z));

    Thread.Sleep(500);
}



