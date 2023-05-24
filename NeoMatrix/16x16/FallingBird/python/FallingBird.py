from DUELink.DUELinkController import DUELinkController
import time
import math
import random

port = DUELinkController.GetConnectionPort()
dueController = DUELinkController(port)

w=16      # Width of the matrix 
u=4       # Player X position
v=8       # Player Y position
t=0       # Player tail Y offset
b=15      # Wall X position
h=4       # Wall height
g=6       # Wall gap
x = 0
y = 0
p = 0


# Handle the player
def plyr():
    global w,u,v,t,b,h,g,x,y,p
    if (dueController.Digital.Read('a', 1)) == False:
        if v > 0:
            v -=1
        t = 1
    else:
        if v < 15:
            v = v + 0.5
        t = -1
    
    x = u
    y = math.trunc(v)
    pxl()
    dueController.Neo.SetColor(p, 0x400040)
    x=x-1
    y=y+t
    pxl()
    dueController.Neo.SetColor(p, 0x400040)
    return


# Update wall
def wall():
    global w,u,v,t,b,h,g,x,y,p
    b=b-0.25
    if (b <= 0):
        b = 15
        g = 4 + random.randrange(2)
        h = 2 + random.randrange(6)

    for i in range (2):
        x = math.trunc(b) + i

        for y in range (h+1):
            pxl()
            dueController.Neo.SetColor(p, 0x004000)
        
        for y in range (h+g, 16):
            pxl()
            dueController.Neo.SetColor(p, 0x004000)
    return        
    
# Check collision
def coll():
    global w,u,v,t,b,h,g,x,y,p
    i = math.trunc(b)       
    if i != u and i != u -1:
        return
    
    if v <= h:
        die()

    if v >= h+g:
        die()

# Player died
def die():
    global w,u,v,t,b,h,g,x,y,p
    for i in range (0, 20):
        x = (u - 2) + random.randrange(4)
        y = (v - 2) + random.randrange(4)

        pxl()
        dueController.Neo.SetColor(p, 0x400000)
        dueController.Neo.Show(1, 256)
    b = 15

# Formula for index into 16x16 NeoPixel Matrix
# p=pxl(x,y)

def pxl():
    global w,u,v,t,b,h,g,x,y,p
    p = x*w+(x&1)*(w-1)+(1-2*(x&1))*y

def Loop():
    while True:
        dueController.Neo.Clear()
        plyr()
        wall()
        coll()
        dueController.Neo.Show(1, 256)
        time.sleep(50/1000)

Loop()




