

using _3DCube;
using GHIElectronics.DUELink;

var port = DUELinkController.GetConnectionPort();
var dev = new DUELinkController(port);

var cube = new Cube3D(dev);

cube.Run(); 
