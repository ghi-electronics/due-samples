// See https://aka.ms/new-console-template for more information
// This project run on BrainPad Pulse

using GHIElectronics.DUE;

using DUE.Graphics;
using static GHIElectronics.DUE.DUEController;

var port = DUEController.GetConnectionPort();
var serial = new SerialInterface(port);
serial.Connect();

var neo = new NeoController(serial);
var btn = new ButtonController(serial);

Canvas canvas = new(16, 16,
    (int x, int y, int w) => ((x * w) + ((x & 1) * (w - 1)) + ((1 - (2 * (x & 1))) * y)) * 3,
    (byte[] pixels) => { neo.Stream(pixels); neo.Show(256); }
);


FallingBirdGame game = new(canvas, btn);
game.Run();
