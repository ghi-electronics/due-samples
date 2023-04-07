using System.Net.NetworkInformation;
using GHIElectronics.DUE;
using LedRing;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var resetPin = 6;
var latchPin = 17;
var dataPin = 16;
var clockPin = 18;

var clickledring = new LedRingController(dueController, resetPin, latchPin, dataPin, clockPin);

while (true) {

    for (var i = 0; i < 32; i++) {
        clickledring.Set(i, true);

        
        clickledring.Show();
    }

    clickledring.Clear();

}
