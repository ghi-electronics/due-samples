using System.Security.Cryptography.X509Certificates;
using GHIElectronics.DUE;
using Gyroscope;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var gyroscope = new GyroscopeController(dueController);

while (true) {
    if (gyroscope.AccelerationAvailable()) {
        gyroscope.ReadAcceleration(out var x, out var y, out var z);

        Console.WriteLine(string.Format("Accel: X = {0}, Y = {1}, Z = {2}", x.ToString("f2"), y.ToString("f2"), z.ToString("f2")));

    }

    Thread.Sleep(500);
}



