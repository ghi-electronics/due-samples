// See https://aka.ms/new-console-template for more information

using DUE.Graphics;

class Particle {
    private readonly Canvas g;
    private Vector2d position;
    private Vector2d velocity;
    private readonly Color from;
    private readonly Color to;
    private int lifeTime;
    private int totalLifeTime;

    public Particle(Canvas g, int x, int y, Color from, Color to, int lifetime) {
        this.g = g;
        this.position = new Vector2d(x, y);
        this.velocity = Vector2d.RandomDirection(1 + (Random.Shared.NextDouble()*2));
        this.from = from;
        this.to = to;
        this.totalLifeTime = lifetime;
        this.lifeTime = 0;
    }

    public void Update() {
        if (this.lifeTime >= this.totalLifeTime) return;
        this.lifeTime++;
        this.g.SetPixel((int)this.position.X, (int)this.position.Y, Color.Lerp(this.from, this.to, (double)this.lifeTime / this.totalLifeTime));
        this.position += this.velocity;
    }

    public bool IsActive => this.lifeTime < this.totalLifeTime;
}
