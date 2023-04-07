
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var screen = new SSD1306.SSD1306(dueController);

// Default i2c address is 0x3C. To use different address:
//var screen = new SSD1306.SSD1306(dueController, 0x3C);

screen.Clear();
screen.DrawString("DUE - SSD1306", 1, 5, 5);

screen.Show();

