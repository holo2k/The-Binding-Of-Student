using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mbox = System.Windows.Forms.MessageBox;

using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System;
using System.Linq;
using The_Binding_Of_Student.Code;
using System.Runtime.CompilerServices;

namespace The_Binding_Of_Student
{
    public class Player 
    {  
        static public SpriteBatch SpriteBatch { get; set; }

        public static Texture2D PersonWalkBack { get; set; }
        public static Texture2D PersonWalkForward { get; set; }
        public static Texture2D PersonWalkLeft { get; set; }
        public static Texture2D PersonWalkRight { get; set; }
        public static Texture2D PersonStanding { get; set; }
        public static Texture2D PersonWalkBackHitted { get; set; }
        public static Texture2D PersonWalkForwardHitted { get; set; }
        public static Texture2D PersonWalkLeftHitted { get; set; }
        public static Texture2D PersonWalkRightHitted { get; set; }
        public static Texture2D PersonStandingHitted { get; set; }
        public static Texture2D FourHearts { get; set; }
        public static Texture2D ThreeHearts { get; set; }
        public static Texture2D TwoHearts { get; set; }
        public static Texture2D OneHeart { get; set; }
        public static Texture2D ZeroHearts { get; set; }

        public static Texture2D Tear { get; set; }

        public static int frameWidth = 165;
        public static int frameHeight = 170;
        
        private static bool hitted = false;
        private static double timeToShoot = 0.0d;
        private static double timeInvinсible = 0.0d;
        private static double hittedTime = 0.0d;

        //Characteristic
        public double fireRate = 0.5d;
        public int bulletSpeed = 10; 
        public static int playerSpeed = 5; 
        public static int healthPoints = 99;
        private bool IsAlive() => healthPoints > 0;

        public static Rectangle playerHitBox = new Rectangle();

        public static int[,] playerMapPos = new int[,]
        {
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0}
        };


        private static Vector2 personPosition = new Vector2();

        public static Vector2 PersonPosition 
        {
            get
            {
                return new Vector2(personPosition.X + 30, personPosition.Y + 130); // центр спрайта
            }
            set
            { 
                personPosition.X = value.X;
                personPosition.Y = value.Y;
            }
        }
        public static Vector2 GetPosForShoot => new Vector2(personPosition.X + frameWidth/2, personPosition.Y + frameHeight/2);

        static Point currentFrame = new Point(0, 0);
        static Point spriteSize = new Point(3, 1);
        static Point spriteSizeStanding = new Point(7, 1);
        static Point tearSize = new Point(0, 0);

        public static List<Bullet> bullets = new List<Bullet>();

        static int currentTime = 0; // сколько времени прошло
        static int period = 140; // период обновления в миллисекундах

        public static Direction Direction { get; set; }

        public static Direction ShootDirection()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left)) return Direction.Left;
            if (keyboardState.IsKeyDown(Keys.Right)) return Direction.Right;
            if (keyboardState.IsKeyDown(Keys.Up)) return Direction.Top;
            if (keyboardState.IsKeyDown(Keys.Down)) return Direction.Bottom;
            return Direction.None;
        }

        public Player()
        {

        }

        public Player(Texture2D walkDown, Texture2D walkUp, Texture2D walkLeft, Texture2D walkRight, Texture2D standing)
        {
            PersonWalkBack = walkDown;
            PersonWalkForward = walkUp;
            PersonWalkLeft = walkLeft;
            PersonWalkRight = walkRight;
            PersonStanding = standing;
        }
        static public void UpdateFrame(GameTime gameTime)
        {
            if (currentTime > period)
            {
                currentTime -= period; // вычитаем из текущего времени период обновления
                ++currentFrame.X; // переходим к следующему фрейму в спрайте
                if (currentFrame.X >= spriteSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= spriteSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }
        static public void UpdateFrameStanding(GameTime gameTime)
        {
            if (currentTime > period)
            {
                currentTime -= period; // вычитаем из текущего времени период обновления
                ++currentFrame.X; // переходим к следующему фрейму в спрайте
                if (currentFrame.X >= spriteSizeStanding.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= spriteSizeStanding.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        

        public void Moving(GameTime gameTime)
        {
            if (IsAlive())
            {
                KeyboardState keyboardState = Keyboard.GetState();
                playerHitBox = new Rectangle((int)personPosition.X, (int)personPosition.Y, frameWidth, frameHeight);

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    personPosition.X -= playerSpeed;
                    UpdateFrame(gameTime);
                    Direction = Direction.Left;
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    personPosition.X += playerSpeed;
                    UpdateFrame(gameTime);
                    Direction = Direction.Right;
                }
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    personPosition.Y -= playerSpeed;
                    UpdateFrame(gameTime);
                    Direction = Direction.Top;
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    personPosition.Y += playerSpeed;
                    UpdateFrame(gameTime);
                    Direction = Direction.Bottom;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.S) && Keyboard.GetState().IsKeyUp(Keys.A) && Keyboard.GetState().IsKeyUp(Keys.W) && Keyboard.GetState().IsKeyUp(Keys.D))
                {
                    currentTime += gameTime.ElapsedGameTime.Milliseconds;
                    UpdateFrameStanding(gameTime);
                    Direction = Direction.None;
                }

                if (personPosition.X < 97) personPosition.X = 97;
                if (personPosition.Y < -10) personPosition.Y = -10;
                if (personPosition.X > 1600) personPosition.X = 1600;
                if (personPosition.Y > 675) personPosition.Y = 675;

               // if ((timeInvinсible += gameTime.ElapsedGameTime.TotalSeconds) > 3.0d)
                if(gameTime.IsTimeElapsed(ref timeInvinсible, 3.0d))
                {
                    for (int i = 0; i < Game1.monsters.Count-1; i++)
                    {
                        if (playerHitBox.Intersects(Game1.monsters[i].monsterPosition, Game1.monsters[i].monsterSize) && Game1.monsters[i].IsInSameRoom(gameTime))
                        {
                            timeInvinсible = 0.0d;
                            healthPoints -= 33;
                            hitted = true;
                        }
                        for (int j = 0; j < Game1.monsters[i].bullets.Count-1; j++)
                        {
                            Game1.monsters[i].bullets[j].Update();
                           if (playerHitBox.Intersects(Game1.monsters[i].bullets[j].Pos, Bullet.size))
                           {
                                Game1.monsters[i].bullets.RemoveAt(j);
                                j--;
                                timeInvinсible = 0.0d;
                                healthPoints -= 33;
                                hitted = true;
                            }
                        }
                    }  
                }
                

                if (healthPoints <= 0)
                {
                    var result = mbox.Show("Вы погибли!", "Уведомление", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                    Game1.gameFinished = true;
                    Game1.state = State.MainMenu;
                }

                if (gameTime.IsTimeElapsed(ref timeToShoot, 0.5d))
                {
                    if (ShootDirection() != Direction.None)
                    {
                        timeToShoot = 0.0d;
                        PlayerFire();
                    }
                }
                
                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Update();
                    if (bullets[i].Hidden)
                    {
                        bullets.RemoveAt(i);
                        i--;
                    }
                }

                
            }
            else
            {
                personPosition = Vector2.Zero;
            }
            
        }

        public void DrawMovement(SpriteBatch spriteBatch, Texture2D personMovement)
        {
            if (IsAlive())
            {
                spriteBatch.Draw(personMovement, personPosition,
                           new Rectangle(currentFrame.X * frameWidth, currentFrame.Y * frameHeight, frameWidth, frameHeight),
                           Color.White,
                           0,
                           Vector2.Zero,
                           1.3f,
                           SpriteEffects.None,
                           1);
            } 
        }

        public  void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive())
            {
                if (!hitted)
                {
                    DrawState(Direction, spriteBatch, PersonStanding, PersonWalkForward, PersonWalkBack, PersonWalkLeft, PersonWalkRight);
                }
                else
                {
                    DrawState(Direction, spriteBatch, PersonStandingHitted, PersonWalkForwardHitted, PersonWalkBackHitted, PersonWalkLeftHitted, PersonWalkRightHitted);
                    if (gameTime.IsTimeElapsed(ref hittedTime, 3.0d))
                    {
                        hitted = false;
                        hittedTime = 0.0d;
                    }
                    
                }

                foreach(Bullet bullet in bullets)
                {
                    bullet.Draw();
                }
            }
            switch (healthPoints)
            {
                case 132:
                    spriteBatch.Draw(FourHearts, new Vector2(165, 70), Color.White);
                    break;
                case 99:
                    spriteBatch.Draw(ThreeHearts, new Vector2(165, 70), Color.White);
                    break;
                case 66:
                    spriteBatch.Draw(TwoHearts, new Vector2(165, 70), Color.White);
                    break;
                case 33:
                    spriteBatch.Draw(OneHeart, new Vector2(165, 70), Color.White);
                    break;
                case <= 0:
                    spriteBatch.Draw(ZeroHearts, new Vector2(165, 70), Color.White);
                    break;
            }
            
        }

        

        void DrawState(Direction dir, SpriteBatch spriteBatch, Texture2D standing, Texture2D wForward, Texture2D wBack, Texture2D wLeft, Texture2D wRight)
        {
            switch (dir)
            {
                case Direction.None:
                    DrawMovement(spriteBatch, standing);
                    Direction = Direction.None;
                    break;
                case Direction.Top:
                    DrawMovement(spriteBatch, wForward);
                    Direction = Direction.None;
                    break;
                case Direction.Bottom:
                    DrawMovement(spriteBatch, wBack);
                    Direction = Direction.None;
                    break;
                case Direction.Left:
                    DrawMovement(spriteBatch, wLeft);
                    Direction = Direction.None;
                    break;
                case Direction.Right:
                    DrawMovement(spriteBatch, wRight);
                    Direction = Direction.None;
                    break;
            }
        }

        void PlayerFire()
        {
            bullets.Add(new Bullet(GetPosForShoot, ShootDirection(), Tear, bulletSpeed));
        }
        
    }
}
