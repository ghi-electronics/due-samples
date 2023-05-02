using System.ComponentModel.DataAnnotations;
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var chipselect = 16; // DUE pin
var controlPin = 12; // DUE pin
var resetPin = 8; // DUE pin
var backlightPin = 1; // DUE pin


var screen = new ST7735.ST7735Controller(dueController, chipselect, controlPin, resetPin, backlightPin);

// Use internal buffer
// user can use external buffer with BasicGraphic
screen.Clear();
screen.DrawString("DUE - ST7735", 0x00FF00, 5, 5);
screen.Show();

Thread.Sleep(1000);

// This for 4bpp - show Pallete
var data = new uint[160 * 128];
byte color = 0;
for (var y = 0; y < 128; y++) {
    for (var x = 0; x < 160; x++) {
        data[y * 160 + x] = color;
    }

    if (y % 8 == 0 && y != 0)
        color++;
}
screen.DrawBuffer(data, 0, data.Length, true);

