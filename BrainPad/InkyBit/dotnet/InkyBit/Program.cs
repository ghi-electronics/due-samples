using GHIElectronics.DUE;
using InkyBit;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var screen = new InkyBit.InkyBit(dueController);

screen.Clear();

screen.DrawString("DUE - InkyBit", 1, 5, 5, 3, 3);

screen.Show();
