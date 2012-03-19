using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Binding.Items
{
    class AnarchistCookbook : SpacebarItem
    {
        Random rand;
        public AnarchistCookbook(Player p, GameManager m)
            : base(p, m)
        {
            rand = GameConst.random;
        }

        public override void Activate()
        {
            int count = 0;
            while (count < 6)
            {
                int x = 800 * (manager.Map.CurrentRoom.XPos) + rand.Next(800 - ((Tile.WIDTH * 2) - Bomb.WIDTH) + Tile.WIDTH);
                int y = 480 * (manager.Map.CurrentRoom.YPos) + rand.Next(480 - ((Tile.HEIGHT * 2) - Bomb.HEIGHT) + Tile.HEIGHT);
                Rectangle testRect = new Rectangle(x, y, Bomb.WIDTH, Bomb.HEIGHT);
                bool colliding = false;
                foreach (Tile tile in manager.Map.CurrentRoom.SolidTiles)
                {
                    if (tile.Rect.Intersects(testRect))
                    {
                        colliding = true;
                        break;
                    }
                }
                if (!colliding)
                {
                    Bomb bomb = new Bomb(new Vector2(x, y),
                        manager, player);
                    bomb.CurrentRoom = manager.Map.CurrentRoom;
                    manager.AddBomb(bomb);
                    count++;
                }
            }
        }
    }
}
