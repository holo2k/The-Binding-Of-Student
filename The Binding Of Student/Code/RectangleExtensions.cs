using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Binding_Of_Student.Code
{
    static class RectangleExtensions
    {
        public static bool Intersects(this Rectangle r1, Vector2 rectanglePos, Point rectangleSize)
        {
            var r2 = new Rectangle((int)rectanglePos.X, (int)rectanglePos.Y, rectangleSize.X, rectangleSize.Y);
            return r1.Intersects(r2);
        }

    }
}
