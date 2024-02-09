using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Binding_Of_Student
{
    public class Bullet
    {
        public Vector2 Pos;
        Direction Dir;
        public static Point size = new Point(50,50);
        int speed = 0;
        Color color = Color.White;
        public Vector2 playerPos;
        Vector2 direction;
        public Texture2D Texture2D;
        public static SpriteBatch SpriteBatch { get; set; }

        public Bullet(Vector2 Pos, Direction Dir, Texture2D texture, int _speed)
        {
            this.Pos = Pos;
            this.Dir = Dir;
            this.Texture2D = texture;
            speed = _speed;
            playerPos = Player.PersonPosition;
            ChooseDirection();
            direction.Normalize();
        }

        void ChooseDirection()
        {
            switch (Dir)
            {
                case Direction.Left:
                    direction = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    direction = new Vector2(+1, 0);
                    break;
                case Direction.Top:
                    direction = new Vector2(0, -1);
                    break;
                case Direction.Bottom:
                    direction = new Vector2(0, +1);
                    break;
                case Direction.ToPlayer:
                   direction = playerPos - Pos;
                    break;
            }
        }

        public bool Hidden
        {
            get
            {
                return (((Pos.X >= Game1.Width || Pos.Y >= Game1.Height) || Pos.X <= 0 || Pos.Y <= 0) || Pos == playerPos);
            }
        }

        public void Update()
        {
            if (!Hidden)
            {
                Pos += direction * speed;
            }
        }

        public void Draw()
        {
            SpriteBatch.Draw(Texture2D, Pos, color);      
        }
    }
}
