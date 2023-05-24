using GHIElectronics.DUELink;
using TM1637;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var pinClk = 5;
var pinDio = 4;

var sevendigits = new TM1637Controller(dueController, pinClk, pinDio) {
    Brightness = 7,    
};

var counter = 1000;

while (true) {
    //sevendigits.Clear();
    sevendigits.ShowNumberDec(counter, true);
    Thread.Sleep(500);
    sevendigits.ShowNumberDec(counter, false);
    Thread.Sleep(500);
    counter++;    
}

