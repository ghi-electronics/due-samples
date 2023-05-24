
using GHIElectronics.DUELink;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var screen = new SSD1306.SSD1306(dueController);

// Default i2c address is 0x3C. To use different address:
//var screen = new SSD1306.SSD1306(dueController, 0x3C);

screen.Clear();
screen.DrawString("DUELink - SSD1306", 1, 5, 5);

screen.Show();

Thread.Sleep(1000);

//Draw half screen black and half screen color by buffer

var data = new uint[128 * 64];

for (var y = 0; y < 64; y++) {
    for (var x = 0; x < 128; x++) {
        if (y > 32)
            data[y * 128 + x] = 0xFFFFFF;
        else
            data[y * 128 + x] = 0x000000;
    }
}

screen.DrawBuffer(data, 0, data.Length);
