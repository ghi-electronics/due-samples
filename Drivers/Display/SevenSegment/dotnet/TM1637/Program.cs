using GHIElectronics.DUE;
using TM1637;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var sevendigits = new TM1637Controller(dueController, 5, 4, 1);

sevendigits.SetBrightness(255, true);

var counter = 1000;

while (true) {
    //sevendigits.Clear();
    sevendigits.ShowNumberDec(counter, true);
    Thread.Sleep(500);
    sevendigits.ShowNumberDec(counter, false);
    Thread.Sleep(500);
    counter++;
    
}

