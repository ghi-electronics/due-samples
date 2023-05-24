
from DUELink.DUELinkController import DUELinkController
import hd44780


availablePort = DUELinkController.GetConnectionPort()

bp = DUELinkController(availablePort)

d = hd44780.HD44780Controller(bp)

d.home()
d.write('Hello world!')
