using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.DUE;
using static GHIElectronics.DUE.DUEController.ButtonController;

namespace SpriteMaster {
    public class SpriteMaster {
        DUEController dueController;

        public SpriteMaster(DUEController bp) {
            this.dueController = bp;

            this.dueController.Button.Enable((int)Buttons.B, true);
            this.dueController.Button.Enable((int)Buttons.A, true);
        }
        public void Run() {


            var frame1 = new int[] { 1,0,0,1,1,1,1,1,0,
                                       0,1,1,1,1,1,1,1,1,
                                       1,0,0,0,0,1,0,0,0,
                                       1,1,1,1,1,1,1,1,1,
                                       0,0,1,0,0,0,0,0,1
            };
            var frame2 = new int[] { 1,0,0,1,1,1,1,1,0,
                                       0,1,1,1,1,1,1,1,1,
                                       1,0,1,0,0,1,1,0,0,
                                       1,1,1,1,1,1,1,1,1,
                                       0,0,1,0,0,0,0,0,1
            };

            var bulletImage = new int[] {1,0,
                                           0,1,
                                           1,0,
                                           0,1
            };


            var playerImage = new int[] {0,0,0,1,1,1,1,0,0,0,0,
                                           0,0,0,1,1,1,1,0,0,0,0,
                                           1,1,1,1,1,1,1,1,1,1,1,
                                           1,1,1,1,1,1,1,1,1,1,1
            };

            bullet.X = 150;
            bullet.Y = 150;
            bullet.Width = 2;
            bullet.Height = 4;
            bullet.Image = bulletImage;

            enemy.X = 10;
            enemy.Y = 10;
            enemy.Width = 9;
            enemy.Height = 5;
            enemy.Image = frame1;

            enemy2.X = 20;
            enemy2.Y = 10;
            enemy2.Width = 9;
            enemy2.Height = 5;
            enemy2.Image = frame2;

            enemy3.X = 30;
            enemy3.Y = 10;
            enemy3.Width = 9;
            enemy3.Height = 5;
            enemy3.Image = frame1;



            player.X = 75;
            player.Y = 60;
            player.Width = 11;
            player.Height = 4;
            player.Image = playerImage;


            var start = DateTime.Now.Ticks;
            var end = DateTime.Now.Ticks;




            while (true) {
                start = DateTime.Now.Ticks;
                basicGfx.Clear(0);
                basicGfx.DrawLine(white, 103, 0, 103, 64);

                for (var i = 0; i < lives; i++) {
                    basicGfx.DrawString("^", white, 110, i * 10);

                }

                basicGfx.DrawString(score.ToString(), white, 110, 55);

                // Process the bullets
                if (bulletIsOut) {

                    bullet.Y -= 12;

                    ;

                    if (bullet.Y < 0) {
                        bulletIsOut = false;

                    }
                    else {
                        
                        this.DrawSprite(bullet);
                        this.dueController.Sound.Play((int)(100 - bullet.Y) * 75, 80, 100);
                    }


                    if (bullet.X >= enemy.X - 3 && bullet.X <= enemy.X + enemy.Width + 3 &&
                        bullet.Y >= enemy.Y - 3 && bullet.Y <= enemy.Y + enemy.Height + 3) {
                        score += 10;
                        enemyAlive = false;
                    }
                    if (bullet.X >= enemy2.X - 3 && bullet.X <= enemy2.X + enemy2.Width + 3 &&
                       bullet.Y >= enemy2.Y - 3 && bullet.Y <= enemy2.Y + enemy2.Height + 3) {
                        score += 10;
                        enemyAlive2 = false;
                    }

                    if (bullet.X >= enemy3.X - 3 && bullet.X <= enemy3.X + enemy3.Width + 3 &&
                      bullet.Y >= enemy3.Y - 3 && bullet.Y <= enemy3.Y + enemy3.Height + 3) {
                        score += 10;
                        enemyAlive3 = false;
                    }
                    if (enemyAlive == false && enemyAlive2 == false && enemyAlive3 == false) {

                        enemyAlive = true;
                        enemyAlive2 = true;
                        enemyAlive3 = true;
                        enemy.X = 10;
                        enemy.Y = 10;

                        enemy2.X = 20;
                        enemy2.Y = 10;

                        enemy3.X = 30;
                        enemy3.Y = 10;

                    }
                }
                else {
                    var buttonPressed = this.dueController.Button.IsPressed((int)Buttons.A);

                    if (!buttonPressed)
                        buttonPressed = this.dueController.Button.IsPressed((int)Buttons.B);

                    if (buttonPressed) {
                        bullet.Y = 64;

                        bullet.X = player.X + 5;

                        bulletIsOut = true;


                    }
                }

                if (enemyAlive) this.DrawSprite(enemy);
                if (enemyAlive2) this.DrawSprite(enemy2);
                if (enemyAlive3) this.DrawSprite(enemy3);
                this.DrawSprite(player);

                var random_rocket = new Random();

                var rockerX = random_rocket.Next(0, 100);

                if (rockerX < 50) player.X += 5;
                if (rockerX > 50) player.X -= 5;


                if (player.X < 0) player.X = 0;
                if (player.X > 90) player.X = 90;

                enemy.X += enemySpeed1;
                enemy2.X += enemySpeed2;
                enemy3.X += enemySpeed3;


                if (enemy.X < 5 || enemy.X > 85) {
                    enemySpeed1 *= -1;
                    enemy.Y += 5;
                }
                if (enemy2.X < 5 || enemy2.X > 85) {
                    enemySpeed2 *= -1;
                    enemy2.Y += 5;
                }
                if (enemy3.X < 5 || enemy3.X > 85) {
                    enemySpeed3 *= -1;
                    enemy3.Y += 5;
                }

                if (enemy.Y > 64 || enemy2.Y > 64 || enemy3.Y > 64) {
                    enemy.X = 10;
                    enemy.Y = 10;

                    enemy2.X = 20;
                    enemy2.Y = 10;

                    enemy3.X = 30;
                    enemy3.Y = 10;
                }
                
                this.dueController.Display.Stream(basicGfx.Buffer);

                // Delay for 20fps max

                end = DateTime.Now.Ticks;
                var diff1 = end - start;

                diff1 = diff1 / 10000;

                if (diff1 < 50) // 1000 / 20fps max = 50ms
                    Thread.Sleep((int)(50 - diff1));
            }
        }

        void DrawSprite(Sprite sprite) {
            var index = 0;
            while (index <= sprite.Image.Length - 1) {
                for (var y = 0; y < sprite.Height; y++) {
                    for (var x = 0; x <= sprite.Width - 1; x++) {
                        if (sprite.Image[index] == 1) {                            
                            basicGfx.SetPixel(white, sprite.X + x, sprite.Y + row);
                        }
                        else {                            
                            basicGfx.SetPixel(black, sprite.X + x, sprite.Y + row);
                        }
                        index++;
                    }
                    row++;
                }
                row = 0;
            }

        }
        static BasicGraphics basicGfx = new BasicGraphics(128, 64);
        static int row = 0;
        static Sprite enemy = new Sprite();
        static Sprite enemy2 = new Sprite();
        static Sprite enemy3 = new Sprite();
        static bool enemyAlive = true;
        static bool enemyAlive2 = true;
        static bool enemyAlive3 = true;
        static Sprite bullet = new Sprite();
        static Sprite player = new Sprite();
        static int score = 0;
        static uint white = 1;
        static uint black = 0;

        static int xShip = 62;
        static int yBullet = 20;
        static int xBullet = 0;
        static bool bulletIsOut = false;
        static int xMonster = 0;
        static int yMonster = 0;
        static int enemySpeed1 = 5;
        static int enemySpeed2 = 5;
        static int enemySpeed3 = 5;

        static int lives = 3;
    }
}
