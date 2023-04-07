using System.Security.Cryptography.X509Certificates;
using GHIElectronics.DUE;
using Gyroscope;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var gyroscope = new GyroscopeController(dueController);

while (true) {
    if (gyroscope.AccelerationAvailable()) {
        gyroscope.ReadAcceleration(out var x, out var y, out var z);

        Console.WriteLine(string.Format("Accel: X = {0}, Y = {1}, Z = {2}", x * gyroscope.AccelerationSampleRate, y, z));

    }

    if (gyroscope.GyroscopeAvailable()) {
        gyroscope.ReadGyroscope(out var x, out var y, out var z);

        Console.WriteLine(string.Format("Gyroscope: X = {0}, Y = {1}, Z = {2}", x, y, z));

    }


    Thread.Sleep(500);
}



