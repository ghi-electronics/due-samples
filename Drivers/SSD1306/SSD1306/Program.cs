
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var screen = new SSD1306.SSD1306(dueController);

// Set left half of screen is 0, right half screen is 1
for (var y = 0; y < screen.Height; y++) {
    for (var x = 0; x < screen.Width; x++) {
        if (x < screen.Width/2) {
            screen.SetPixel(x, y, false);
        }
        else {
            screen.SetPixel(x, y, true);
        }
    }
}

screen.Flush();

