
from DUE.DUEController import DUEController
import hd44780


availablePort = DUEController.GetConnectionPort()

bp = DUEController(availablePort)

d = hd44780.HD44780Controller(bp)

d.home()
d.write('Hello world!')
