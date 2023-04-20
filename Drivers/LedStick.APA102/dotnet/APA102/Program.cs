using System.Drawing;
using APA102;
using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var ledstick = new APA102Controller(dueController);

var colors = new int[] { 0xFFFFFF, 0xFF0000, 0x00FF00, 0x0000FF };

var colorIndex = 0;

while (true) {

    var red = (byte)(colors[colorIndex % 4] >> 16);
    var green = (byte)(colors[colorIndex % 4] >> 8);
    var blue = (byte)(colors[colorIndex % 4] >> 0);

    for (var i = 0; i < ledstick.LedCount; i++) {
        if (i ==0) {
            ledstick.Set(ledstick.LedCount-1, 0, 0, 0);
        }
        else
            ledstick.Set(i - 1, 0, 0, 0);

        ledstick.Set(i,red, green,blue);

        //Thread.Sleep(100);
    }
    colorIndex++;
}

