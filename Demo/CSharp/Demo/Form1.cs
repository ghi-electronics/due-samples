
using GHIElectronics.DUE;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btSound_Click(object sender, EventArgs e)
        {
            var port = DUEController.GetConnectionPort();

            var dev = new DUEController(port);

            dev.Sound.Play(1000, 100, 100);

            dev.Disconnect();
        }

        private void ledOn_Click(object sender, EventArgs e)
        {
            var port = DUEController.GetConnectionPort();

            var dev = new DUEController(port);

            dev.Digital.Write((int)DUEController.Pin.Led, true);

            dev.Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var port = DUEController.GetConnectionPort();

            var dev = new DUEController(port);

            dev.Digital.Write((int)DUEController.Pin.Led, false);

            dev.Disconnect();
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            var port = DUEController.GetConnectionPort();

            var dev = new DUEController(port);

            dev.Display.Clear(0);



            dev.Display.DrawTextScale(  this.sendtxt.Text , 1, 0, 0, 1, 1);

            dev.Display.Show();

            dev.Disconnect();
        }
    }
}
