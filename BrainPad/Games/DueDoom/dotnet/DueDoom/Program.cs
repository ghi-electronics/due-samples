// See https://aka.ms/new-console-template for more information

using DueDoom;
using GHIElectronics.DUELink;

var port = DUELinkController.GetConnectionPort();
var dueController = new DUELinkController(port);

var gfx = new BasicGraphics(128, 64);


var game = new Doom(dueController, gfx);
game.Run();




