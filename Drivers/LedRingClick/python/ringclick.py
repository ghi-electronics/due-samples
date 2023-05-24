from DUELink.DUELinkController import DUELinkController
from DUELink.Digital import Input



class RingClickController: 
    INPUT = 0
    OUTPUT = 1

    def __init__(self, dueController: DUELinkController, reset: int, latch: int, data: int, clock: int) -> None:
        self.dueController = dueController
        self.resetPin = reset
        self.latchPin = latch
        self.dataPin = data
        self.clockPin = clock
        self.ledIndex = 0

        self.dueController.Digital.Write(self.resetPin, False)
        self.dueController.Digital.Write(self.resetPin, True)
       
    def __SoftSPI(self, b):
        for i in range(7, -1, -1):
            if ((b & (1 << i)) > 0):
                self.dueController.Digital.Write(self.dataPin, True)
            else:
                self.dueController.Digital.Write(self.dataPin, False)
            self.__Clock()
            print(i)

    def Clear(self) :
        self.ledIndex = 0

    def Show(self):
        self.__SoftSPI(self.ledIndex >> 0)
        self.__SoftSPI(self.ledIndex >> 8)
        self.__SoftSPI(self.ledIndex >> 16)
        self.__SoftSPI(self.ledIndex >> 24)
        self.__Latch()

    def Set(self, id: int, on: bool) :
        if (on) :
            self.ledIndex |= (1 << id)
        else :
            self.ledIndex &= ~(1 << id)

    def __Latch(self):
        self.dueController.Digital.Write(self.latchPin, True)
        self.dueController.Digital.Write(self.latchPin, False)

    def __Clock(self):
        self.dueController.Digital.Write(self.clockPin, True)
        self.dueController.Digital.Write(self.clockPin, False)
        
        
        
            
    

        
        

            
        
        
        
        
     
     
    
        
        
    
        
        
          
    

        
        

        

        





