
using GHIElectronics.DUE;
using LedBar;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var pinClock = 4U;
var pinDio = 5U;
var ledbar = new LedBarController(dueController, pinClock, pinDio, true, LedType.MaxLed10);

while (true) {
    for (var i = 0U; i < 10; i++) {
        ledbar.SetLed(i, 255);
    }

    for (var i = 0U; i < 10; i++) {
        ledbar.SetLed(i, 0);
    }
}
