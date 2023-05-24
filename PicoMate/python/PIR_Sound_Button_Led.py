# We need to connect pin 1 (Neo pin) to the RGB led
# Button is connected to pin 26
# PIR connected to pin 28
# Buzzer connect to pin 27
# Default the led is green color
# When user press the button, the color is while, a beep sound will be played.
# When uset release the button, a beep sound will be played.
# When a motion detected, the led will be changed to red

# 0.96 OlED Display: Refer to https://github.com/ghi-electronics/due-samples/tree/main/Drivers/Display/SSD1306
# Temperature and Humidity: Refer to https://github.com/ghi-electronics/due-samples/tree/main/Drivers/Temperature-Humidity/SHT31
# Gyroscope Accelerometer : https://github.com/ghi-electronics/due-samples/tree/main/Drivers/Accelerometer/Gyro

from DUELink.DUELinkController import DUELinkController
from DUELink.Digital import Input
import time

availablePort = DUELinkController.GetConnectionPort()

dueController = DUELinkController(availablePort)

buttonPressed = False
moveDetected = False

dueController.Neo.SetColor(0, 0x00FF00) # Led Green
dueController.Neo.Show(1)

buttonPin = 26
pirPin = 28
beepPin = 27

while (True) :
    buttonState = not dueController.Digital.Read(buttonPin, Input.PULL_UP)
    pirState = dueController.Digital.Read(pirPin, Input.PULL_UP)

    if (buttonPressed != buttonState) :
        buttonPressed = buttonState

        if (buttonPressed):
            dueController.Neo.SetColor(0, 0xFFFFFF); # led while
        
        else :
            dueController.Neo.SetColor(0, 0x00FF00); # led green
        

        dueController.Neo.Show(1)

        dueController.System.Beep(beepPin, 1000, 100)

        print("Button Pressed " + str(buttonPressed))

    

    if (moveDetected != pirState) :
        moveDetected = pirState

        if (moveDetected) :
            dueController.Neo.SetColor(0, 0xFF0000) # led read
            print("Moving detected!")
        
        else :
            dueController.Neo.SetColor(0, 0x00FF00) # led green
            print("Moving stopped!")
        

        dueController.Neo.Show(1)


    




