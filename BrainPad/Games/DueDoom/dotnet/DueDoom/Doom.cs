using GHIElectronics.DUE;

namespace DueDoom {
    internal partial class Doom {
        const int MaxRenderDepth = 12;
        const double TurnSpeed = 0.05;
        const double WalkSpeed = 0.1;

        readonly DUEController dueController;
        readonly BasicGraphics gfx;

        public Doom(DUEController dueController, BasicGraphics gfx) {
            this.dueController = dueController;
            this.gfx = gfx;

            this.dueController.Button.Enable((int)DUEController.Pin.ButtonA, true);
            this.dueController.Button.Enable((int)DUEController.Pin.ButtonB, true);
        }

        public void Run() {
            var posX = 1.5;
            var posY = 1.5;
            var dirX = 1.0;
            var dirY = 0.0;
            var planeX = 0.0;
            var planeY = 0.66;

            while (true) {
                this.gfx.Clear(0);
                for (var x = 0; x < this.gfx.Width; x++) {
                    var cameraX = 2 * x / (double)this.gfx.Width - 1;
                    var rayDirX = dirX + planeX * cameraX;
                    var rayDirY = dirY + planeY * cameraX;

                    var mapX = (int)posX;
                    var mapY = (int)posY;

                    double sideDistX;
                    double sideDistY;

                    var deltaDistX = (rayDirX == 0) ? 1e30 : Math.Abs(1 / rayDirX);
                    var deltaDistY = (rayDirY == 0) ? 1e30 : Math.Abs(1 / rayDirY);
                    double perpWallDist;

                    int stepX;
                    int stepY;

                    var side = 0;

                    if (rayDirX < 0) {
                        stepX = -1;
                        sideDistX = (posX - mapX) * deltaDistX;
                    }
                    else {
                        stepX = 1;
                        sideDistX = (mapX + 1.0 - posX) * deltaDistX;
                    }

                    if (rayDirY < 0) {
                        stepY = -1;
                        sideDistY = (posY - mapY) * deltaDistY;
                    }
                    else {
                        stepY = 1;
                        sideDistY = (mapY + 1.0 - posY) * deltaDistY;
                    }

                    var hit = false;
                    var depth = 0;
                    while (!hit && depth < MaxRenderDepth) {
                        if (sideDistX < sideDistY) {
                            sideDistX += deltaDistX;
                            mapX += stepX;
                            side = 0;
                        }
                        else {
                            sideDistY += deltaDistY;
                            mapY += stepY;
                            side = 1;
                        }
                        if (mapY >= this.map.GetLowerBound(0) && mapY <= this.map.GetUpperBound(0) &&
                            mapX >= this.map.GetLowerBound(1) && mapX <= this.map.GetUpperBound(1)) {
                            hit = this.map[mapY, mapX] != 0;
                        }
                        depth++;
                    }

                    if (side == 0) perpWallDist = (sideDistX - deltaDistX);
                    else perpWallDist = (sideDistY - deltaDistY);

                    var lineHeight = (int)(this.gfx.Height / perpWallDist);


                    var drawStart = -lineHeight / 2 + this.gfx.Height / 2;
                    if (drawStart < 0) drawStart = 0;
                    var drawEnd = lineHeight / 2 + this.gfx.Height / 2;
                    if (drawEnd >= this.gfx.Height) drawEnd = this.gfx.Height - 1;

                    double distance;
                    if (side == 0) {
                        distance = Math.Max(1, (mapX - posX + (1 - stepX) / 2) / rayDirX);
                    }
                    else {
                        distance = Math.Max(1, (mapY - posY + (1 - stepY) / 2) / rayDirY);
                    }
                    this.DrawSliver(x, drawStart, drawEnd, GradientCount - (int)(distance / MaxRenderDepth * GradientCount) - side * 2);
                }
                this.dueController.Display.DrawBuffer(this.gfx.Buffer, 0, this.gfx.Buffer.Length);

                double turn = 0;
                double walk = 0;

                if (Console.KeyAvailable) {
                    var key = Console.ReadKey(false).Key;

                    switch (key) {
                        case ConsoleKey.LeftArrow:
                            turn = -TurnSpeed;
                            break;
                        case ConsoleKey.RightArrow:
                            turn = TurnSpeed;
                            break;
                        case ConsoleKey.UpArrow:
                            walk = WalkSpeed;
                            break;
                        case ConsoleKey.DownArrow:
                            walk = -WalkSpeed;
                            break;
                    }

                    if (this.map[(int)posY, (int)(posX + dirX * walk)] == 0) posX += dirX * walk;
                    if (this.map[(int)(posY + dirY * walk), (int)posX] == 0) posY += dirY * walk;

                    var oldDirX = dirX;
                    dirX = dirX * Math.Cos(turn) - dirY * Math.Sin(turn);
                    dirY = oldDirX * Math.Sin(turn) + dirY * Math.Cos(turn);

                    var oldPlaneX = planeX;
                    planeX = planeX * Math.Cos(turn) - planeY * Math.Sin(turn);
                    planeY = oldPlaneX * Math.Sin(turn) + planeY * Math.Cos(turn);
                }

                if (turn == 0 && walk == 0) { // process button only if keyboard not used.

                    var btA = this.dueController.Button.IsPressed((int)DUEController.Pin.ButtonA) ? true : false;
                    var btB = this.dueController.Button.IsPressed((int)DUEController.Pin.ButtonB) ? true : false;
                    if (btA && btB) {
                        walk = WalkSpeed;
                        if (this.map[(int)posY, (int)(posX + dirX * walk)] == 0) posX += dirX * walk;
                        if (this.map[(int)(posY + dirY * walk), (int)posX] == 0) posY += dirY * walk;
                    }
                    else {


                        if (btA) {
                            turn = -TurnSpeed;
                        }
                        else if (btB) {
                            turn = TurnSpeed;
                        }

                        var oldDirX = dirX;
                        dirX = dirX * Math.Cos(turn) - dirY * Math.Sin(turn);
                        dirY = oldDirX * Math.Sin(turn) + dirY * Math.Cos(turn);

                        var oldPlaneX = planeX;
                        planeX = planeX * Math.Cos(turn) - planeY * Math.Sin(turn);
                        planeY = oldPlaneX * Math.Sin(turn) + planeY * Math.Cos(turn);
                    }
                }
            }
        }

        void DrawSliver(int x, int startY, int endY, int intensity) {
            var lower_y = Math.Max(Math.Min(startY, endY), 0);
            var higher_y = Math.Min(Math.Max(startY, endY), this.gfx.Height - 1);

            for (var y = lower_y; y <= higher_y; y++) {
                for (var c = 0; c < 2; c++) {
                    if (this.GetGradientPixel(x + c, y, intensity)) {
                        this.gfx.SetPixel(1, x + c, y);
                    }
                }

            }
        }
    }
}
