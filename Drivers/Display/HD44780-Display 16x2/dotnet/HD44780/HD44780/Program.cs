using GHIElectronics.DUELink;
using HD44780;

var port = DUELinkController.GetConnectionPort();

var dueController = new DUELinkController(port);

var lcd = new HD44780Controller(dueController);

var text = "Hello world!";

lcd.Clear();    

lcd.SetCursor(0, 0);
lcd.DrawText(text);


// auto scrool
//lcd.SetCursor(16, 1);
//lcd.AutoScroll = true;

//for (var i = 0; i < text.Length; i++) {
//    lcd.DrawChar(text[i]);
//    Thread.Sleep(500);
//}



