using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Binding.Items
{
    class Teleport : SpacebarItem
    {
        Random rand;
        public Teleport(Player p, GameManager m)
            : base(p, m)
        {
            rand = GameConst.random;
        }

        public override void Activate()
        {
            Room warpRoom = null;

            while (warpRoom == null
                || warpRoom == player.CurrentRoom)
            {
                int x = rand.Next(Map.ROOMSW);
                int y = rand.Next(Map.ROOMSH);

                warpRoom = manager.Map.Rooms[x, y];
            }

            player.RoomWarp(warpRoom.XPos, warpRoom.YPos);
        }
    }
}
