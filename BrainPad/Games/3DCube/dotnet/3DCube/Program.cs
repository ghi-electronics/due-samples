

using _3DCube;
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();
var dev = new DUEController(port);

var cube = new Cube3D(dev);

cube.Run(); 
