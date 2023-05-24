using GHIElectronics.DUELink;
using InkyBit;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var screen = new InkyBit.InkyBit(dueController);

screen.Clear();

screen.DrawString("DUELink - InkyBit", 1, 5, 5, 3, 3);

screen.Show();
