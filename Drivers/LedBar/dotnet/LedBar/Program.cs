
using GHIElectronics.DUE;
using LedBar;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var pinClock = 4;
var pinDio = 5;
var ledbar = new LedBarController(dueController, pinClock, pinDio);

while (true) {
    for (var i = 0; i < ledbar.LedNum; i++) {
        ledbar.SetLed(i, 255);
    }

    for (var i = 0; i < ledbar.LedNum; i++) {
        ledbar.SetLed(i, 0);
    }
}
