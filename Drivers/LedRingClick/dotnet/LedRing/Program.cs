// IMPORTANT: Click shield for Pi Pico has a major flaw. MISO and MOSI pins are swapped on board rev 1.00
// The code does not use SPI and instead bit-bang to give soft-SPI on the compensated pins to correct the design error
// This works but the system will run extremely slow

using GHIElectronics.DUELink;
using LedRing;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

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
