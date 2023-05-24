using GHIElectronics.DUELink;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var spritemaster = new SpriteMaster.SpriteMaster(dueController);

spritemaster.Run();
