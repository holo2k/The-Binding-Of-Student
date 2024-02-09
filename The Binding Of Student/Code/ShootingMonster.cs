using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace The_Binding_Of_Student
{
    public class ShootingMonster : Monster
    {
        private bool isHitted = false;
        private double timeInvincible = 0d;
        private double timeToShoot = 0d;
        private double hittedTime = 0d;
        public static Vector2 currentPos = Vector2.Zero;
        static int currentTime = 0;
        public static Texture2D acid { get; set; }


        private static Vector2 GetPosForShoot => new Vector2(currentPos.X + 50, currentPos.Y+50);
        public ShootingMonster(Point _size, Vector2 position, Vector2 _mapPos)
        {
            monsterSize = _size;
            monsterPosition = position;
            mapPos = _mapPos;
            healthpoints = 70;
        }

        public override void Moving(GameTime gameTime, int speed)
        {
            if (IsInSameRoom(gameTime))
            {
                if ((timeInvincible += gameTime.ElapsedGameTime.TotalSeconds) > 0.5d)
                {
                    for (int i = 0; i < Player.bullets.Count; i++)
                    {
                        Player.bullets[i].Update();
                        if (Collide(Player.bullets[i].Pos, Bullet.size))
                        {
                            Player.bullets.RemoveAt(i);
                            i--;
                            timeInvincible = 0.0d;
                            healthpoints -= 10;
                            isHitted = true;
                        }
                    }
                }

                currentPos = monsterPosition;

                if ((timeToShoot += gameTime.ElapsedGameTime.TotalSeconds) > 1.5d)
                {
                    timeToShoot = 0.0d;
                    MonsterFire();
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
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isHitted)
            {
                DrawState(spriteBatch, hittedMonster2);
                if ((hittedTime += gameTime.ElapsedGameTime.TotalSeconds) > 0.5d)
                {
                    isHitted = false;
                    hittedTime = 0.0d;
                }
            }
            else
            {
                DrawState(spriteBatch, monster2);
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw();
                
            }
        }

        public void MonsterFire()
        {
            bullets.Add(new Bullet(GetPosForShoot, Direction.ToPlayer, acid, 4));
        }
    }
}
