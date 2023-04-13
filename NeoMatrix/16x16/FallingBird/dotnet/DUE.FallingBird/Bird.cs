// See https://aka.ms/new-console-template for more information


using DUE.Graphics;
using static GHIElectronics.DUE.DUEController;
using static GHIElectronics.DUE.DUEController.ButtonController;

partial class Bird {
    private const int FALL_DELAY = 2;

    private readonly Canvas g;
    private readonly ButtonController btn;
    private readonly List<Particle> particles = new();
    private readonly int startX;
    private readonly int startY;

    private int y;
    private int dy = 0;
    private int fallTimer;
    private States state = States.Alive;

    private enum States {
        Alive,
        Die,
        Exploding,
        Dead
    }

    public Bird(Canvas g, ButtonController btn, int x, int y) {
        this.g = g;
        this.btn = btn;
        this.startX = x;
        this.startY = this.y = y;

        btn.Enable((int)Buttons.B, true);
    }

    public void Reset() {
        this.y = this.startY;
        this.state = States.Alive;
        this.dy = 0;
    }

    public void Update() {
        switch (this.state) {
            case States.Alive:
                this.Fly();
                break;
            case States.Die:
                this.Die();
                this.state = States.Exploding;
                break;
            case States.Exploding:
                this.Explode();
                break;
            case States.Dead:
                break;
        }
    }

    private void Fly() {
        if (this.btn.IsPressed((int)Buttons.B)) {
            this.fallTimer = FALL_DELAY;
            if (this.y > 0) {
                this.y--;
                this.dy = 1;
            }
        }
        else {
            this.fallTimer--;
        }

        if (this.fallTimer <= 0 && this.y < this.g.Height - 1) {
            this.fallTimer = FALL_DELAY;
            this.y++;
            this.dy = -1;
        } else if ( this.y == this.g.Height - 1) {
            this.dy = 0;
        }

        if (this.g.GetPixel(this.startX, this.y) == Color.Green) {
            this.state = States.Die;
        }
        else {
            this.g.SetPixel(this.startX, this.y, Color.Magenta);
            this.g.SetPixel(this.startX - 1, this.y + this.dy, Color.Magenta);
        }
    }

    private void Die() {
        this.particles.Clear();
        for(var i=0; i<50; i++) {
            this.particles.Add(new Particle(this.g, this.startX, this.y, Color.Yellow, Color.Red, 4 + Random.Shared.Next(4)));
        }
    }

    private void Explode() {
        if (this.particles.Count == 0) {
            this.state = States.Dead;
            return;
        }

        for (var i = this.particles.Count-1; i >= 0; i--) {
            var particle = this.particles[i];
            particle.Update();
            if (!particle.IsActive) {
                this.particles.RemoveAt(i);
            }
        }
    }

    public bool Dead => this.state == States.Dead;
}
