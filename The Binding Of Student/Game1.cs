using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using The_Binding_Of_Student.Code;

namespace The_Binding_Of_Student
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static State state = State.MainMenu;
        public static List<Monster> monsters = new List<Monster>();
        public static bool gameFinished = true;
        public static bool exit = false;
        public static int Width = 1920;
        public static int Height = 1080;
        private int speed;
        KeyboardState keyboardState, oldKeyboardState;

        public static Player player = new Player();
        public static Room currentRoom;
        
        public static int[,] playerPos = new int[,]
        { 
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0}
        };

        public int count = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MainMenu.Background = Content.Load<Texture2D>("backgrondGame");
            MainMenu.BtnPlay = Content.Load<Texture2D>("btnPlay");
            MainMenu.BtnSettings = Content.Load<Texture2D>("btnSettings");
            MainMenu.BtnExit = Content.Load<Texture2D>("btnExit");
            MainMenu.BtnPlayPressed = Content.Load<Texture2D>("btnPlayPressed");
            MainMenu.BtnSettingsPressed = Content.Load<Texture2D>("btnSettingsPressed");
            MainMenu.BtnExitPressed = Content.Load<Texture2D>("btnExitPressed");
            MainMenu.btnPlaySize = new Point(Content.Load<Texture2D>("btnPlay").Width, Content.Load<Texture2D>("btnPlay").Height);
            MainMenu.btnSettingsSize = new Point(Content.Load<Texture2D>("btnSettings").Width, Content.Load<Texture2D>("btnSettings").Height);
            MainMenu.btnExitSize = new Point(Content.Load<Texture2D>("btnExit").Width, Content.Load<Texture2D>("btnExit").Height);
            InformationMenu.Background = Content.Load<Texture2D>("Information");
            InformationMenu.BtnBack = Content.Load<Texture2D>("backButton");
            InformationMenu.BtnBackPressed = Content.Load<Texture2D>("btnBackPressed");
            Player.PersonWalkBack = Content.Load<Texture2D>("walkBack");
            Player.PersonWalkForward = Content.Load<Texture2D>("walkForward");
            Player.PersonWalkLeft = Content.Load<Texture2D>("walkLeft");
            Player.PersonWalkRight = Content.Load<Texture2D>("walkRight");
            Player.PersonStanding = Content.Load<Texture2D>("standing");
            Player.PersonWalkBackHitted = Content.Load<Texture2D>("walkBackHitted");
            Player.PersonWalkForwardHitted = Content.Load<Texture2D>("walkForwardHitted");
            Player.PersonWalkLeftHitted = Content.Load<Texture2D>("walkLeftHitted");
            Player.PersonWalkRightHitted = Content.Load<Texture2D>("walkRightHitted");
            Player.PersonStandingHitted = Content.Load<Texture2D>("standingHitted");
            Player.Tear = Content.Load<Texture2D>("Tear");
            Monster.monster1 = Content.Load<Texture2D>("monster1");
            Monster.hittedMonster1 = Content.Load<Texture2D>("monster1Hitted");
            Monster.monster2 = Content.Load<Texture2D>("monster2");
            Monster.hittedMonster2 = Content.Load<Texture2D>("monster2Hitted");
            Player.SpriteBatch = _spriteBatch;
            ShootingMonster.acid = Content.Load<Texture2D>("Acid");
            ShootingMonster.SpriteBatch = _spriteBatch;
            Bullet.SpriteBatch = _spriteBatch;
            Player.FourHearts = Content.Load<Texture2D>("4Hearts");
            Player.ThreeHearts = Content.Load<Texture2D>("3Hearts");
            Player.TwoHearts = Content.Load<Texture2D>("2Hearts");
            Player.OneHeart = Content.Load<Texture2D>("1Heart");
            Player.ZeroHearts = Content.Load<Texture2D>("0Hearts");
            Room.RoomBackground = Content.Load<Texture2D>("gameRoom");
            Door.DoorLeft = Content.Load<Texture2D>("leftDoor");
            Door.DoorRight = Content.Load<Texture2D>("rightDoor");
            Door.DoorUp = Content.Load<Texture2D>("upDoor");
            Door.DoorDown = Content.Load<Texture2D>("downDoor");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (exit)
            {
                Exit();
            }
            switch (state)
            {
                case State.MainMenu:
                    MainMenu.Update();
                    break;
                case State.Game:
                    if (keyboardState.IsKeyDown(Keys.Escape)) state = State.MainMenu;
                    if(!gameFinished)
                    {
                        player.Moving(gameTime);
                        currentRoom.Update();
                        monsters = currentRoom.monsters;
                        if (monsters.Count > 0)
                        {
                            for (int i = 0; i < monsters.Count - 1; i++)
                            {
                                monsters[i].Moving(gameTime, 3);
                                if (!monsters[i].IsAlive())
                                {
                                    monsters.Remove(monsters[i]);
                                }
                            }
                        }
                    }
                    if (keyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R) && gameFinished == false)
                    {
                        LevelGeneration.Start();
                    }
                    break;
                case State.Information:
                    InformationMenu.Update();
                    break;
                case State.End:

                    break;
            }
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            switch (state)
            {
                case State.MainMenu:
                    MainMenu.Draw(_spriteBatch);
                    break;
                case State.Game:
                    if (gameFinished == false)
                    {
                        currentRoom.Draw(_spriteBatch);
                        player.Draw(_spriteBatch, gameTime);
                        if (monsters.Count > 0)
                        {
                            for (int i = 0; i < monsters.Count - 1; i++)
                            {
                                monsters[i].Draw(_spriteBatch, gameTime);
                            }
                        }
                    }
                       
                    break;
                case State.Information:
                    InformationMenu.Draw(_spriteBatch);
                    break;
                case State.End:
                    
                    break;
            }
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}