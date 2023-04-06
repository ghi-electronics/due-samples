
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var screen = new SSD1306.SSD1306(dueController);

screen.Clear();
screen.DrawString("DUE - SSD1306", 1, 5, 5);

screen.Flush();

