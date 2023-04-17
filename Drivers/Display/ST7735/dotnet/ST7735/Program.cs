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

// This for 4bpp - show Pallete
var data = new byte[160 * 128 / 2];
byte color = 0;

for (var i = 0; i < data.Length; i += 640) {
    for (var c = i; c < i + 640; c++) {
        data[c] = (byte)((color << 4) | color);
    }

    color++;
}
screen.Show(data, true);

