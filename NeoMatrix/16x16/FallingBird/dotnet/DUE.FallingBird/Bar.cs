// See https://aka.ms/new-console-template for more information

using DUE.Graphics;

class Bar {
    public const int BAR_UPDATE_FRAME = 5;
    private const int MIN_GAP = 4;
    private const int MAX_GAP = 7;
    private const int MIN_HEIGHT = 3;
    private const int MAX_HEIGHT = 15 - MIN_HEIGHT;

    private readonly int gap;
    private readonly int height;
    private readonly Canvas g;

    private int x;

    public Bar(Canvas g, int x) {
        this.g = g;
        this.gap = MIN_GAP + Random.Shared.Next(MAX_GAP - MIN_GAP);
        this.height = MIN_HEIGHT + Random.Shared.Next(MAX_HEIGHT - this.gap);
        this.x = x; 
    }

    public void Update() {
        for (var i = 0; i < 2; i++) {
            this.g.DrawLine(this.x + i, 0, this.x + i, this.height - 1, Color.Green);
            this.g.DrawLine(this.x + i, this.height + this.gap, this.x + i, this.g.Height - 1, Color.Green);
        }

        if (FallingBirdGame.Frames % BAR_UPDATE_FRAME == 0) this.x -= 1;
    }

    public bool IsActive => x >= 0;
}
