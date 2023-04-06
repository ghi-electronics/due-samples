

using GHIElectronics.DUE;

namespace Accel
{
    public class Program
    {
        static void Main(string[] args)
        {
            var port = DUEController.GetConnectionPort();

            var dueController = new DUEController(port);

            var accel = new Accel(dueController);

            while (true) {
                Console.WriteLine(string.Format("value: X= {0}, Y= {1}, Z= {2}", accel.X, accel.Y, accel.Z));
                Thread.Sleep(100);
            }
        }
    }
}

