
using System.Runtime.CompilerServices;
using GHIElectronics.DUE;

using SmartHome;
var pageIndex = 0;
var musicEnabled = false;
var showTemperature = false;
var showHumidity = false;
var musicIdx = 0;
var lightEnabled = false;
var fanAuto = false;
var showFan = false;
var doorOpening = false;
var windowOpening = false;


var port = DUEController.GetConnectionPort();

var dueController = new DUEController(port);

var fan = new FanController(dueController, 12, 13);

var led = new SingleLedController(dueController, 3);
var rgbled = new RgbLedController(dueController);

var window = new WindowController(dueController, 8);

var door = new DoorController(dueController, 11);

var music = new MusicController(dueController);

var temperature = new TemperatureController(dueController);


fan.TurnOff();
rgbled.TurnOff();
window.Close();
door.Close();
music.Stop();

dueController.Infrared.Enable(true);

var keyPressed = 0;


while (true) {

    if (keyPressed >= 0 && keyPressed < 26) {

        switch (keyPressed) {
            default:
                ResetVariables(false);
                if ((pageIndex % 2) == 0) {
                    dueController.Display.Clear(0);
                    dueController.Display.DrawText("**Smart Home by DUE**", 1, 0, 0);
                    dueController.Display.DrawText("key 0: Fan auto/off", 1, 0, 10);
                    dueController.Display.DrawText("key 1..3: Fan speead", 1, 0, 20);
                    dueController.Display.DrawText("up/down: door control", 1, 0, 30);
                    dueController.Display.DrawText("left/right: window", 1, 0, 40);
                    dueController.Display.DrawText("back/forward:pre/next", 1, 0, 50);


                }
                else if ((pageIndex % 2) == 1) {
                    dueController.Display.Clear(0);
                    dueController.Display.DrawText("Smart home enabled!", 1, 0, 0);
                    dueController.Display.DrawText("key 4: Show Temp.", 1, 0, 10);
                    dueController.Display.DrawText("key 5: Show Humidity", 1, 0, 20);
                    dueController.Display.DrawText("speaker key: Music", 1, 0, 30);
                    dueController.Display.DrawText("Lightbub key: control", 1, 0, 40);
                    dueController.Display.DrawText("control", 1, 0, 50);
                }

                pageIndex++;

                dueController.Display.Show();
                break;

            case 1:
            case 9:
                ResetVariables(false);

                doorOpening = !doorOpening;
                if (doorOpening) {
                    door.Open();
                    dueController.Display.Clear(1);
                    dueController.Display.DrawTextScale("Door", 0, 15, 5, 2, 2);
                    dueController.Display.DrawTextScale("opened!", 0, 15, 30, 2, 2);
                    dueController.Display.Show();
                }
                else {
                    door.Close();
                    dueController.Display.Clear(0);
                    dueController.Display.DrawTextScale("Door", 1, 15, 5, 2, 2);
                    dueController.Display.DrawTextScale("closed!", 1, 15, 30, 2, 2);
                    dueController.Display.Show();
                }
                break;

            case 4:
            case 6:
                ResetVariables(false);
                windowOpening = !windowOpening;

                if (windowOpening) {
                    window.Open();
                    dueController.Display.Clear(1);
                    dueController.Display.DrawTextScale("Window", 0, 15, 5, 2, 2);
                    dueController.Display.DrawTextScale("opened!", 0, 15, 30, 2, 2);
                    dueController.Display.Show();
                }
                else {
                    window.Close();
                    dueController.Display.Clear(0);
                    dueController.Display.DrawTextScale("Window", 1, 15, 5, 2, 2);
                    dueController.Display.DrawTextScale("closed!", 1, 15, 30, 2, 2);
                    dueController.Display.Show();
                }
                break;


            case 20:
                ResetVariables(false);

                showTemperature = !showTemperature;


                break;
            case 21:
                ResetVariables(false);

                showHumidity = !showHumidity;

                break;

            case 5:
                ResetVariables(true);
                musicEnabled = !musicEnabled;


                dueController.Display.Clear(0);
                dueController.Display.DrawTextScale("Music:" + (musicEnabled ? "On" : "Off"), 1, 5, 5, 2, 2);

                dueController.Display.Show();

                break;

            case 2:
                ResetVariables(false);
                lightEnabled = !lightEnabled;

                if (lightEnabled) {

                    led.TurnOn();
                    rgbled.TurnOn(255, 255, 255);
                    dueController.Display.Clear(1);
                    dueController.Display.DrawTextScale("Light on", 0, 5, 5, 2, 2);

                    dueController.Display.Show();
                }
                else {
                    led.TurnOff();
                    rgbled.TurnOff();
                    dueController.Display.Clear(0);
                    dueController.Display.DrawTextScale("Light off", 1, 5, 5, 2, 2);
                    dueController.Display.Show();
                }



                break;

            case 13:
                ResetVariables(false);
                fanAuto = !fanAuto;
                showFan = true;

                if (!fanAuto) {
                    dueController.Display.Clear(0);
                    dueController.Display.DrawTextScale("Fan Auto", 1, 5, 5, 2, 2);
                    dueController.Display.DrawTextScale("OFF", 1, 30, 30, 3, 3);
                    dueController.Display.Show();

                    fan.TurnOff();
                }

                break;

            case 16:
                ResetVariables(false);
                fanAuto = false;

                fan.TurnOn(40, false);
                dueController.Display.Clear(0);
                dueController.Display.DrawTextScale("Fan Speed", 1, 5, 5, 2, 2);
                dueController.Display.DrawTextScale("1", 1, 56, 30, 4, 4);
                dueController.Display.Show();
                break;

            case 17:
                ResetVariables(false);
                fanAuto = false;

                fan.TurnOn(60, false);
                dueController.Display.Clear(0);
                dueController.Display.DrawTextScale("Fan Speed", 1, 5, 5, 2, 2);
                dueController.Display.DrawTextScale("2", 1, 56, 30, 4, 4);
                dueController.Display.Show();
                break;
            case 18:
                ResetVariables(false);
                fanAuto = false;

                fan.TurnOn(80, false);
                dueController.Display.Clear(0);
                dueController.Display.DrawTextScale("Fan Speed", 1, 5, 5, 2, 2);
                dueController.Display.DrawTextScale("3", 1, 56, 30, 4, 4);
                dueController.Display.Show();
                break;

        }
    }

    if (musicEnabled) {
        music.Play(musicIdx, 250);
        musicIdx++;

        if (musicIdx == MusicController.Tones.Length) {
            musicIdx = 0;
        }
    }
    if (showTemperature) {
        dueController.Display.Clear(0);
        dueController.Display.DrawTextScale("Temperature:", 1, 5, 5, 1, 1);
        dueController.Display.DrawTextScale(temperature.Temperature.ToString("F2") + " C", 1, 0, 30, 3, 3);
        dueController.Display.Show();
    }
    else if (showHumidity) {
        dueController.Display.Clear(0);
        dueController.Display.DrawTextScale("Humidity:", 1, 5, 5, 1, 1);
        dueController.Display.DrawTextScale(temperature.Humidity.ToString("F2") + "%", 1, 0, 30, 3, 3);
        dueController.Display.Show();
    }
    else if (fanAuto) {
        if (temperature.Temperature > 30) {
            fan.TurnOn(80, false);

         
        }
        else if (temperature.Temperature > 26) {
            fan.TurnOn(50, false);

           
        }
        else {
            fan.TurnOff();
           
        }
        if (showFan) {
            dueController.Display.Clear(0);
            dueController.Display.DrawTextScale("Fan Auto", 1, 5, 5, 2, 2);
            dueController.Display.DrawTextScale(temperature.Temperature.ToString("F2") + " C", 1, 0, 30, 3, 3);
            dueController.Display.Show();
        }

    }

    Thread.Sleep(100);


    keyPressed = dueController.Infrared.Read();
    if (keyPressed != -1)
        Console.WriteLine("Key pressed: " + keyPressed);
}

void ResetVariables(bool ignoremusic) {
    showTemperature = false;
    showHumidity = false;
    if (!ignoremusic)
        musicEnabled = false;
    showFan = false;
}
