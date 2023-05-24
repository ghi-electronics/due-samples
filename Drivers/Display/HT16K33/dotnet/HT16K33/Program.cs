using GHIElectronics.DUELink;
using HT16K33;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var ht16k33 = new HT16K33Controller(dueController) {
    Rotation = 0,
    Brightness = 15,
};


ht16k33.Clear();
ht16k33.DrawCharacter('D', 1, 0, 0);
ht16k33.Show();

