using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var screen = new ScrollBit.ScrollBit(dueController);

screen.Clear();
screen.DrawString("DUE", 128, 0, 0);
screen.Show();
