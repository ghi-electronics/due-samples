using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var spritemaster = new SpriteMaster.SpriteMaster(dueController);

spritemaster.Run();
