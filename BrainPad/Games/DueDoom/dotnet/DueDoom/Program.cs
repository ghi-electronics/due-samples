// See https://aka.ms/new-console-template for more information

using DueDoom;
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();
var dueController = new DUEController(port);

var gfx = new BasicGraphics(128, 64);


var game = new Doom(dueController, gfx);
game.Run();




