using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using The_Binding_Of_Student.Code;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace The_Binding_Of_Student
{
    public class Room
    {

        public static Texture2D RoomBackground { get; set; }
        public Vector2 mapPos;
        public int type;
        public bool doorTop = false, doorBot = false, doorLeft = false, doorRight  = false;
          
        static Vector2 playerPos = new Vector2(550, 200);
        static Vector2 monsterPos;
        static Vector2 tearPos = new Vector2(370, 400);
        static Vector2 acidPos = new Vector2(1920 / 3, 900);

        public static Point btnPlaySize { get; set; }
        public static Point btnExitSize { get; set; }
        public static Point btnSettingsSize { get; set; }

        public List<Door> doors = new List<Door>();
        public List<Monster> monsters = new List<Monster>();
        Random rnd = new Random();

        public Room(Vector2 _mapPos, int _type, int monstersCount)
        {
            //monsters.Add(new Monster());
            GenerateMonsters(_mapPos, monstersCount);
            mapPos = _mapPos;
            type = _type;
        }

        public void GenerateMonsters(Vector2 _mapPos, int monstersCount)
        {
            int num;
            if (monstersCount == 0)
            {
                num = rnd.Next(0, 6);
                if (num == 6) ;
                monsters.Add(new Monster());
                //спавн хпшек
            }
            else if (monstersCount == 1)
            {
                monsters.AddRange(new[]
                                    { new ShootingMonster (new Point(100, 90),
                                                   new Vector2(860,460), _mapPos),
                                      new Monster()
                                    });
            }
            else if (monstersCount == 2)
            {
                num = rnd.Next(1, 3);
                switch (num)
                {
                    case 1:
                        monsters.AddRange(new[]
                                    { new Monster (new Point(76, 100),
                                                   new Vector2(200,200), _mapPos),
                                      new Monster (new Point(76, 100),
                                                   new Vector2(1500,700), _mapPos),
                                      new Monster()
                                    });
                        break;
                    case 2:
                        monsters.AddRange(new[]
                                    { new ShootingMonster (new Point(100, 90),
                                                   new Vector2(200,250), _mapPos),
                                      new ShootingMonster (new Point(100, 90),
                                                   new Vector2(1500,250), _mapPos),
                                      new Monster()
                                    });
                        break;
                }

            }
            else if (monstersCount == 3)
            {
                monsters.AddRange(new[]
                                    { new ShootingMonster (new Point(100, 90),
                                                   new Vector2(860,460), _mapPos),
                                      new Monster (new Point(76, 100),
                                                   new Vector2(200,200), _mapPos),
                                      new Monster (new Point(76, 100),
                                                   new Vector2(1500,700), _mapPos),
                                      new Monster()
                                    });
            }
        }

        public void Update()
        {
            foreach (var door in doors)
            {
                door.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RoomBackground, Vector2.Zero, Color.FromNonPremultiplied(255, 255, 255, 256));
            foreach (var door in doors)
            {
                door.Draw(spriteBatch);
            }
        }  
        
        public void UpdateState()
        {
            foreach (var door in doors)
            {
                switch (door.Dir)
                {
                    case Direction.Top:
                        doorTop = true;
                        break;
                    case Direction.Bottom:
                        doorBot = true;
                        break;
                    case Direction.Left:
                        doorLeft = true;
                        break;
                    case Direction.Right:
                        doorRight = true;
                        break;
                }
                
            }
        }
    }

    public class Door
    {
        bool IsOpen() => Game1.monsters.Count == 1;
        public Vector2 Pos;
        public Point size;
        public Direction Dir;
        public Texture2D texture;
        public static Texture2D DoorUp { get; set; }
        public static Texture2D DoorDown { get; set; }
        public static Texture2D DoorLeft { get; set; } 
        public static Texture2D DoorRight { get; set; }

        KeyboardState keyboardState, oldKeyboardState;

        Random rnd = new Random();
        public Door(Direction dir)
        {
            Dir = dir;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            switch (Dir)
            {
                case Direction.Top:
                    Pos = new Vector2(850, 0);
                    size = new Point(166, 186);
                    spriteBatch.Draw(DoorUp, Pos, Color.FromNonPremultiplied(255, 255, 255, 256));
                    break;
                case Direction.Bottom:
                    Pos = new Vector2(850, 900);
                    size = new Point(166, 180);
                    spriteBatch.Draw(DoorDown, Pos, Color.FromNonPremultiplied(255, 255, 255, 256));
                    break;
                case Direction.Left:
                    Pos = new Vector2(0, 450);
                    size = new Point(179, 165);
                    spriteBatch.Draw(DoorLeft, Pos, Color.FromNonPremultiplied(255, 255, 255, 256));
                    break;
                case Direction.Right:
                    Pos = new Vector2(1730, 450);
                    size = new Point(181, 167);
                    spriteBatch.Draw(DoorRight, Pos, Color.FromNonPremultiplied(255, 255, 255, 256));
                    break;
            }
        }

        public void Update()
        {
            keyboardState = Keyboard.GetState();
            if (IsOpen() && 
               (Player.playerHitBox.Intersects(Pos, size) || 
               (Player.playerHitBox.Intersects(new Vector2(Pos.X, Pos.Y - 150), size) && Dir == Direction.Bottom)) &&
               keyboardState.IsKeyDown(Keys.Space) && 
               oldKeyboardState.IsKeyUp(Keys.Space))
            {
                int x;
                int y;
                switch (Dir)
                {
                    case Direction.Top:
                        x = (int)Game1.currentRoom.mapPos.X - 1;
                        y = (int)Game1.currentRoom.mapPos.Y;
                        if (LevelGeneration.rooms[x, y] != null) Game1.currentRoom = LevelGeneration.rooms[x, y];
                        else throw new ArgumentNullException("This room doesn't exist");
                        ChangePlayerPos();
                        Player.playerMapPos[x, y] = 1;
                        break;
                    case Direction.Bottom:
                        x = (int)Game1.currentRoom.mapPos.X + 1;
                        y = (int)Game1.currentRoom.mapPos.Y;
                        if (LevelGeneration.rooms[x, y] != null) Game1.currentRoom = LevelGeneration.rooms[x, y];
                        else throw new ArgumentNullException("This room doesn't exist");
                        ChangePlayerPos();
                        Player.playerMapPos[x, y] = 1;
                        break;
                    case Direction.Left:
                        x = (int)Game1.currentRoom.mapPos.X;
                        y = (int)Game1.currentRoom.mapPos.Y - 1;
                        if (LevelGeneration.rooms[x, y] != null) Game1.currentRoom = LevelGeneration.rooms[x, y];
                        else throw new ArgumentNullException("This room doesn't exist");
                        ChangePlayerPos();
                        Player.playerMapPos[x, y] = 1;
                        break;
                    case Direction.Right:
                        x = (int)Game1.currentRoom.mapPos.X;
                        y = (int)Game1.currentRoom.mapPos.Y + 1;
                        if (LevelGeneration.rooms[x, y] != null) Game1.currentRoom = LevelGeneration.rooms[x, y];
                        else throw new ArgumentNullException("This room doesn't exist");
                        ChangePlayerPos();
                        Player.playerMapPos[x, y] = 1;
                        break;
                }
            }
            oldKeyboardState = keyboardState;
        }

        public void ChangePlayerPos()
        {
            float x = Player.PersonPosition.X;
            if (Dir != Direction.Top && Dir != Direction.Bottom) x *= 2;
            float y = Player.PersonPosition.Y * 2 - 150;
            Player.PersonPosition = new Vector2(Game1.Width - x, Game1.Height - y);
            Player.bullets.Clear();
            Array.Clear(Player.playerMapPos, 0, Player.playerMapPos.Length);
        }

    }
}
