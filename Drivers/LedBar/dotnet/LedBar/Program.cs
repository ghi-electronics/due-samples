
using GHIElectronics.DUELink;
using LedBar;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var pinClock = 4;
var pinDio = 5;
var ledbar = new LedBarController(dueController, pinClock, pinDio);

while (true) {
    ledbar.ReverseShow = !ledbar.ReverseShow;
    for (var i = 0; i < ledbar.LedNum; i++) {
        ledbar.SetLed(i, 255);
        ledbar.Show();
    }

    for (var i = 0; i < ledbar.LedNum; i++) {
        ledbar.SetLed(i, 0);
        ledbar.Show();
    }


}
