from DUE.DUEController import DUEController

import time

availablePort = DUEController.GetConnectionPort()
    
dev = DUEController(availablePort)


def TurnLed(on: bool):
    if (on):
        dev.Digital.Write(108, 1)
    else:
        dev.Digital.Write(108, 0)

def PlaySound():
    dev.Sound.Play(1000, 100, 100)

def SendText(text: str):
    dev.Display.Clear(0)

    if len(text) > 11:
        scale_x = 1
        scale_y = 1
    elif len(text) > 7:
        scale_x = 2
        scale_y = 2
    else:
        scale_x = 3
        scale_y = 3

    
    dev.Display.DrawTextScale(text, 1, 0, 0, scale_x, scale_y)
    dev.Display.Show()

def ShowMenu():
    ret = False

    while (ret == False):
        print("1. Send \"Hellow world!\" to DUE device (to Pulse only)")
        print("2. Play sound (on Pulse only)")
        print("3. Turn the led on")
        print("4. Turn the led off")
        print("5. Exit the demo")                
        c = input("Enter your choice:")
        v = int(c)
        
        match v:
            case 1:
                SendText("Hello world!")
            case 2: 
                PlaySound()
            case 3:
                TurnLed(True)
            case 4:
                TurnLed(True)
            case 5:
                ret = True
ShowMenu()




            

        


        



