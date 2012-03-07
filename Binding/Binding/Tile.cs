using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    enum TileType
    {
        solid,
        background,
        rock,
        door
    }

    class Tile
    {
        public static int WIDTH = 53;
        public static int HEIGHT = 53;
        Texture texture;
        Rectangle rect;
        TileType type;

        public Tile(int x, int y, TileType t)
        {
            rect = new Rectangle(x,y,53,53);
            type = t;
        }

        public void Translate(int x, int y)
        {
            rect.X += x;
            rect.Y += y;
        }

        public void Destroy()
        {
            this.type = TileType.background;
        }

        public Rectangle Rect
        {
            get { return rect; }
        }
        
        public TileType Type
        {
            get { return type; }
        }

        public bool IsSolid()
        {
            if(type == TileType.solid ||
                type == TileType.rock || 
                type == TileType.door)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            switch (type)
            {
                case TileType.solid:
                    sb.Draw(GameConst.blank, rect, Color.PowderBlue);
                    break;
                case TileType.rock:
                    sb.Draw(GameConst.blank, rect, Color.MidnightBlue);
                    break;
                case TileType.door:
                    sb.Draw(GameConst.blank, rect, Color.SteelBlue);
                    break;
            }
        }
    }
}
