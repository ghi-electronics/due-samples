// See https://aka.ms/new-console-template for more information

using DUE.Graphics;
using GHIElectronics.DUE;
using static GHIElectronics.DUE.DUEController;
using static GHIElectronics.DUE.DUEController.ButtonController;

class FallingBirdGame {
    const int MIN_BAR_SPACING = 10;

    public static int Frames { get; private set; }

    readonly List<Bar> bars;

    private readonly Canvas g;
    private readonly ButtonController btn;
    private readonly Bird bird;

    enum States {
        Start,
        Play,
        GameOver
    }

    private States state = States.Start;
    private int lives = 4;
    private int framesToNextBar;
    
    public FallingBirdGame(Canvas g, ButtonController btn) {
        this.g = g;
        this.bird = new Bird(g, btn, 5, 8);
        this.bars = new() {
            new Bar(g, g.Width - 1)
        };
        this.btn = btn; 
    }

    public void Run() {
        this.framesToNextBar = RandomFramesToNextBar();
        while (true) {
            Frames++;
            this.g.Clear();

            switch (this.state) {
                case States.Start:
                    this.UpdateStart();
                    break;
                case States.Play:
                    this.UpdatePlay();
                    break;
                case States.GameOver:
                    this.UpdateGameOver();
                    break;
            }
            this.g.Render();
        }
    }

    private void UpdateGameOver() {
        this.g.DrawText(Font.Font4x6, "Game\nOver", 0, 2, Color.Red);
        if (Frames > 50) this.state = States.Start;
    }

    private void UpdatePlay() {
        for (var i = this.bars.Count - 1; i >= 0; i--) {
            var bar = this.bars[i];
            bar.Update();
            if (!bar.IsActive) this.bars.RemoveAt(i);
        }

        if (--this.framesToNextBar == 0) {
            this.framesToNextBar = RandomFramesToNextBar();
            this.bars.Add(new Bar(this.g, this.g.Width - 1));
        }
        this.bird.Update();

        if (this.bird.Dead) {
            this.Reset(false);
            if (--this.lives == 0) {
                this.state = States.GameOver;
            }
        }
    }

    private void UpdateStart() {
        this.g.DrawText(Font.Font4x6, "'B'", 2, 2, Color.Cyan);
        this.g.DrawText(Font.Font4x6, "Play", 0, 8, Color.Cyan);
        if (this.btn.IsPressed((int)DUEController.Pin.ButtonB)) {
            this.Reset(true);
            this.state = States.Play;
        }
    }

    private void Reset(bool newGame) {
        Frames = 0;
        this.bird.Reset();
        this.bars.Clear();
        if (newGame) {
            this.lives = 4;
        }
    }

    private static int RandomFramesToNextBar() => (MIN_BAR_SPACING + Random.Shared.Next(5)) * Bar.BAR_UPDATE_FRAME;
}
