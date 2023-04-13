using System.Diagnostics;
using DUE.Graphics;

namespace DUE.Game {
    public class Game : GameObject {
        public static Canvas? Canvas { get; private set; }
        public static int Frames { get; private set; }

        public void Run(Canvas canvas) {
            if (Canvas is null) throw new ArgumentNullException(nameof(Canvas));
            Canvas = canvas;

            Frames = 0;
            var lastUpdate = DateTime.Now;
            while (true) {
                Frames++;
                var dt = (DateTime.Now - lastUpdate).TotalSeconds;
                this.Update(Canvas, dt);
                lastUpdate = DateTime.Now;
            }
        }
    }
}
