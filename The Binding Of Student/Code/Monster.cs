using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using The_Binding_Of_Student.Code;

namespace The_Binding_Of_Student
{
    public class Monster
    {
        private static  Random rnd = new Random();
        public Vector2 monsterPosition = Vector2.Zero;
        public static Texture2D monster1 { get; set; }
        public static Texture2D hittedMonster1 { get; set; }
        public static Texture2D monster2 { get; set; }
        public static Texture2D hittedMonster2 { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public List<Bullet> bullets = new List<Bullet>();

        bool isHitted = false;
        protected Vector2 direction;
        protected Vector2 mapPos;
        public Point monsterSize;
        Point currentFrame = new Point(0, 0);
        protected int healthpoints = 50;
        double timeBeforeShoot = 0.0d;
        public bool IsAlive() => healthpoints >= 0;
        public bool IsInSameRoom(GameTime gameTime)
        {
            if (mapPos == Game1.currentRoom.mapPos && gameTime.IsTimeElapsed(ref timeBeforeShoot, 0.5d))
            {
                return true;
            } 
            return false;
        }
        
        protected double hittedTime = 0.0d;
        private Direction currentDir = (Direction)rnd.Next(0, 4);
        private static double timeInvincible = 0.0d;

        protected static int currentTime = 0; // сколько времени прошло

        protected static Direction Direction { get; set; }


        public Monster()
        {

        }

        public Monster(Point _size, Vector2 _position, Vector2 _mapPos)
        {
            monsterSize = _size;
            monsterPosition = _position;
            mapPos = _mapPos;
            
            direction.Normalize();
        }

        public virtual void Moving(GameTime gameTime, int speed)
        {
            if (IsInSameRoom(gameTime))
            {
                direction = Player.PersonPosition - monsterPosition;
                direction.Normalize();
                if ((timeInvincible += gameTime.ElapsedGameTime.TotalSeconds) > 0.6d)
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
                if (isHitted) monsterPosition += direction * -speed / 2;
                else monsterPosition += direction * speed;

                if (monsterPosition.X < 97) monsterPosition.X = 97;
                if (monsterPosition.Y < -10) monsterPosition.Y = -10;
                if (monsterPosition.X > 1600) monsterPosition.X = 1600;
                if (monsterPosition.Y > 675) monsterPosition.Y = 675;
            }
        }

        protected bool Collide(Vector2 entityPos, Point entitySize)
        {
            Rectangle monster = new Rectangle((int)monsterPosition.X, (int)monsterPosition.Y, monsterSize.X, monsterSize.Y);
            Rectangle entity = new Rectangle((int)entityPos.X, (int)entityPos.Y, entitySize.X, entitySize.Y);

            return monster.Intersects(entity);
        }
        public void DrawState(SpriteBatch spriteBatch, Texture2D state)
        {
            spriteBatch.Draw(state, monsterPosition,
                            new Rectangle(currentFrame.X * monsterSize.Y, currentFrame.Y * monsterSize.Y, monsterSize.X, monsterSize.Y),
                            Color.White,
                            0,
                            Vector2.Zero,
                            1.3f,
                            SpriteEffects.None,
                            1);
        }

        
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isHitted)
            {
                DrawState(spriteBatch, hittedMonster1);
                if ((hittedTime += gameTime.ElapsedGameTime.TotalSeconds) > 0.5d)
                {
                    isHitted = false;
                    hittedTime = 0.0d;
                }   
            }
            else
            {
                DrawState(spriteBatch, monster1);
            }
        }

    }
}
