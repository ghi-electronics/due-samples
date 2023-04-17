using GHIElectronics.DUE;
using HT16K33;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var ht16k33 = new HT16K33Controller(dueController) {
    Rotation = 0,
    Brightness = 15,
};


ht16k33.Clear();
ht16k33.DrawCharacter('D', 1, 0, 0);
ht16k33.Show();

