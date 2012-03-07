using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class HUD
    {
        GameManager manager;

        public HUD(GameManager m)
        {
            manager = m;
        }

       

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(0, 0, 800, 120), new Color(45, 45, 45));

            int posX = 20;
            int posY = 20;
            int width = 20;
            int height = 10;

            for (int x = 0; x < Map.ROOMSW; x++)
            {
                for (int y = 0; y < Map.ROOMSW; y++)
                {
                    if (manager.Map.RoomExists(x, y))
                    {
                        sb.Draw(GameConst.blank, 
                            new Rectangle(posX + (x * width), posY + (y * height), 
                                width, height),
                            Color.DarkGray);
                        if(manager.Map.CurrentRoom == manager.Map.GetRoom(x,y))
                        {
                            sb.Draw(GameConst.blank, 
                            new Rectangle(posX + (x * width), posY + (y * height), 
                                width, height),
                            Color.Yellow);
                        }
                    }
                }
            }
        }
    }
}
