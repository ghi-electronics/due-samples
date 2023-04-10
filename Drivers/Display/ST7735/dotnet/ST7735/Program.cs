using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var chipselect = 0; // DUE pin
var controlPin = 1; // DUE pin
var resetPin = 2; // DUE pin
var backlightPin = 3; // DUE pin

var screen = new ST7735.ST7735Controller(dueController, chipselect, controlPin, resetPin, backlightPin);

// Use internal buffer
screen.Clear();
screen.DrawString("DUE - ST7735", 1, 5, 5);
screen.Show();

// User can pass external buffer 16bpp or 4bpp
// void Show(externalbuffer) // 16bpp
// void Show(externalbuffer, true) // 4bpp
