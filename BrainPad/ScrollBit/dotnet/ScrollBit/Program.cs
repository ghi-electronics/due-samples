using GHIElectronics.DUELink;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var screen = new ScrollBit.ScrollBitController(dueController);

screen.Clear();
screen.DrawString("DUELink", 128, 0, 0);
screen.Show();
