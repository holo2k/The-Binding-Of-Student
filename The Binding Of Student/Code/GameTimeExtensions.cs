using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Binding_Of_Student.Code
{
    public static class GameTimeExtensions
    {
        public static bool IsTimeElapsed(this GameTime gameTime, ref double counter, double elapsedTime)
        {
            return (counter += gameTime.ElapsedGameTime.TotalSeconds) > elapsedTime;
        }


    }
}
