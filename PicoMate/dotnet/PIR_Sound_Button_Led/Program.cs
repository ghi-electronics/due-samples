// We need to connect pin 1 (Neo pin) to the RGB led
// Button is connected to pin 26
// PIR connected to pin 28
// Buzzer connect to pin 27
// Default the led is green color
// When user press the button, the color is while, a beep sound will be played.
// When a motion detected, the led will be changed to red

// 0.96 OlED Display: Refer to https://github.com/ghi-electronics/due-samples/tree/main/Drivers/Display/SSD1306
// Temperature and Humidity: Refer to https://github.com/ghi-electronics/due-samples/tree/main/Drivers/Temperature-Humidity/SHT31
// Gyroscope Accelerometer : https://github.com/ghi-electronics/due-samples/tree/main/Drivers/Accelerometer/Gyro

using GHIElectronics.DUE;

var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);


var buttonPressed = false;
var moveDetected = false;

dueController.Neo.SetColor(0, 0, 255, 0); // Led Green
dueController.Neo.Show(1);

var buttonPin = 26;
var pirPin = 28;
var beepPin = 27;

while (true) {
    var buttonState = !dueController.Digital.Read(buttonPin, DUEController.Input.PULL_UP);
    var pirState = dueController.Digital.Read(pirPin, DUEController.Input.PULL_UP);

    if (buttonPressed != buttonState) {
        buttonPressed = buttonState;

        if (buttonPressed) {
            dueController.Neo.SetColor(0, 255, 255, 255); // led while
        }
        else {
            dueController.Neo.SetColor(0, 0, 255, 0); // led green
        }

        dueController.Neo.Show(1);

        dueController.System.Beep(beepPin, 1000, 100);

        Console.WriteLine("Button Pressed " + buttonPressed);

    }

    if (moveDetected != pirState) {
        moveDetected = pirState;

        if (moveDetected) {
            dueController.Neo.SetColor(0, 255, 0, 0); // led read
            Console.WriteLine("Moving detected!");
        }
        else {
            dueController.Neo.SetColor(0, 0, 255, 0); // led green
            Console.WriteLine("Moving stopped!");
        }

        dueController.Neo.Show(1);


    }

}



