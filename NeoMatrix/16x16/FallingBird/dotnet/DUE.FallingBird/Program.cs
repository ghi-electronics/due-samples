// See https://aka.ms/new-console-template for more information
// This project run on BrainPad Pulse

using GHIElectronics.DUELink;

using DUELink.Graphics;
using static GHIElectronics.DUELink.DUELinkController;

var port = DUELinkController.GetConnectionPort();
var dueController = new DUELinkController(port);


//var serial = new SerialInterface(port);
//serial.Connect();

var neo = dueController.Neo;
var btn = dueController.Digital;

Canvas canvas = new(16, 16,
    (int x, int y, int w) => ((x * w) + ((x & 1) * (w - 1)) + ((1 - (2 * (x & 1))) * y)),
    (uint[] pixels) => { neo.SetMultiple(1, pixels); neo.Show(1, 256); }
);

FallingBirdGame game = new(canvas, btn);
game.Run();
