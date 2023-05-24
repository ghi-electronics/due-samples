
using GHIElectronics.DUELink;
using LIS3DH;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var lis3dh = new LIS3DHController(dueController);


while (true) {    
    Console.WriteLine(string.Format("X = {0}, Y = {1}, Z = {2}", lis3dh.X.ToString("0.00"), lis3dh.Y.ToString("0.00"), lis3dh.Z.ToString("0.00")));

    Thread.Sleep(500);
}
