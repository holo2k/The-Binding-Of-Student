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
    public class InformationMenu
    {
        public Game1 game;
        private static MouseState mstate { get; set; }
        private MouseState lastmstate { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        public static Texture2D BtnBack { get; set; }
        public static Texture2D BtnBackPressed { get; set; }

        static Vector2 btnBackPos = new Vector2(890, 930);

        public static Point btnBackSize = new Point(198,70);
        public static Texture2D Background { get; set; }
        static Rectangle btnrec = new Rectangle((int)btnBackPos.X, (int) btnBackPos.Y, btnBackSize.X, btnBackSize.Y);
        static bool IsIntersects() => btnrec.Intersects(new Vector2(mstate.X, mstate.Y), new Point(5, 5));

        public static void Update()
        {
            mstate = Mouse.GetState();
        }

        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            
            if (IsIntersects())
            {
                spriteBatch.Draw(BtnBackPressed, btnBackPos, Color.White);
                if (mstate.LeftButton == ButtonState.Pressed)
                {
                    MainMenu.timeCounter = 230;
                    MainMenu.switched = true;
                    Game1.state = State.MainMenu;
                }
            }
            else
                spriteBatch.Draw(BtnBack, btnBackPos, Color.White);

        }
    }
}


