using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var lm358 = new LM358.LM358Controller(dueController, 28);
// The light sensor is scaled from 0 to 100 as analog read in.
while (true) {
    Console.WriteLine(lm358.Read());

    Thread.Sleep(1000);
}
