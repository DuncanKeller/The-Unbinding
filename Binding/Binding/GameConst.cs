using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Binding
{
    class GameConst
    {
        public static Random random;
        public static ContentManager Content;
        public static Texture2D blank;

        public static void Initialize(ContentManager C)
        {
            random = new Random();
            Content = C;
            blank = C.Load<Texture2D>("white");
        }
    }
}
