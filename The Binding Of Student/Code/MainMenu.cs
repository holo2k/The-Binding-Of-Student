using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mbox = System.Windows.Forms.MessageBox;
using SharpDX.Win32;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using The_Binding_Of_Student.Code;
using System;

namespace The_Binding_Of_Student
{
    public class MainMenu
    {
        public Game1 game;
        private static MouseState mstate { get; set; }
        private MouseState lastmstate { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        public static Texture2D BtnPlay { get; set; }
        public static Texture2D BtnSettings { get; set; }
        public static Texture2D BtnExit { get; set; }
        public static Texture2D BtnPlayPressed { get; set; }
        public static Texture2D BtnSettingsPressed { get; set; }
        public static Texture2D BtnExitPressed { get; set; }

        static Vector2 btnPlayPos = new Vector2(550, 200);
        static Vector2 btnSettingsPos = new Vector2(370, 400);
        static Vector2 btnExitPos = new Vector2(1920 / 3, 900);

        public static Point btnPlaySize { get; set; }
        public static Point btnExitSize { get; set; }
        public static Point btnSettingsSize { get; set; }
        public static Texture2D Background { get; set; }
        static int timeCounter = 0;
        static Color color;

        public static void Update()
        {
            mstate = Mouse.GetState();
            if (timeCounter != 256)
            {
                color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 256);
                timeCounter++;
            }
            else
            {
                timeCounter = 200;
            }
        }

        protected static bool Collide(Vector2 position, Point size)
        {
            Rectangle btnrec = new Rectangle((int)position.X, (int)position.Y, size.X, size.Y);
            Rectangle mouse = new Rectangle(mstate.X, mstate.Y, 5, 5);

            return btnrec.Intersects(mouse);
        }
        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Vector2.Zero, color);
            
            if (Collide(btnPlayPos, btnPlaySize))
            {
                DrawButtons(spriteBatch, BtnPlayPressed, BtnSettings, BtnExit);
                mstate = Mouse.GetState();
                if (Game1.gameFinished == true && mstate.LeftButton == ButtonState.Pressed)
                {
                    LevelGeneration.Start();
                    Game1.state = State.Game;

                }
                else if(Game1.gameFinished == false && mstate.LeftButton == ButtonState.Pressed)
                {
                    Game1.state = State.Game;
                }
                    
            }
            else if (Collide(btnSettingsPos, btnSettingsSize))
            {
                DrawButtons(spriteBatch, BtnPlay, BtnSettingsPressed, BtnExit);
            }
            else if (Collide(btnExitPos, btnExitSize))
            {
                DrawButtons(spriteBatch, BtnPlay, BtnSettings, BtnExitPressed);
                if (mstate.LeftButton == ButtonState.Pressed)
                {
                    var result = mbox.Show("Уверены, что хотите выйти?\nВесь игровой прогресс удалится.", "Уведомление", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        Game1.exit = true;
                    }
                }
            }
            else
            {
                DrawButtons(spriteBatch, BtnPlay, BtnSettings, BtnExit);
            }
        }

        static private void DrawButtons(SpriteBatch spriteBatch, Texture2D btnPlay, Texture2D btnSettings, Texture2D btnExit)
        {
            spriteBatch.Draw(btnPlay, btnPlayPos, Color.White);
            spriteBatch.Draw(btnSettings, btnSettingsPos, Color.White);
            spriteBatch.Draw(btnExit, btnExitPos, Color.White);
        }
    }
}


