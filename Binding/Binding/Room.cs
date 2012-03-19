using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Binding
{
    class Room
    {
        const int TILEW = 15;
        const int TILEH = 9;
        const int ROOMSW = 10;
        const int ROOMSH = 6;

        int xPos;
        int yPos;
        bool completed = false;

        Tile[,] tiles;

        public int XPos
        {
            get { return xPos; }
        }
        public int YPos
        {
            get { return yPos; }
        }

        public bool Compelted
        {
            get { return completed; }
            set { completed = value; }
        }

        public Room(Tile[,] t, int xOffset, int yOffset)
        {
            xPos = xOffset;
            yPos = yOffset;
            tiles = new Tile[TILEW, TILEH];
            for (int x = 0; x < TILEW; x++)
            {
                for (int y = 0; y < TILEH; y++)
                {
                    TileType f = t[x, y].Type;
                    tiles[x, y] = new Tile((xOffset * 800) + x * 53, (yOffset * 480) + y * 53, t[x, y].Type);
                }
            }
        }

        public void AddDoors(Map m)
        {
            if (m.North != null)
            {
                AddDoor("top");
            }
            if (m.South != null)
            {
                AddDoor("bottom");
            }
            if (m.West != null)
            {
                AddDoor("right");
            }
            if (m.East != null)
            {
                AddDoor("left");
            }
        }

        public void RemoveDoors(Map m)
        {
            if (m.North != null)
            {
                RemoveDoor("top");
            }
            if (m.South != null)
            {
                RemoveDoor("bottom");
            }
            if (m.West != null)
            {
                RemoveDoor("left");
            }
            if (m.East != null)
            {
                RemoveDoor("right");
            }
        }

        public void RemoveDoor(string dir)
        {
            //top, bottom, left, right
            int x = 0, y = 0;
            switch (dir)
            {
                case "top":
                    x = TILEW / 2;
                    y = 0;
                    break;
                case "bottom":
                    x = TILEW / 2;
                    y = TILEH - 1;
                    break;
                case "left":
                    x = 0;
                    y = TILEH / 2;
                    break;
                case "right":
                    x = TILEW - 1;
                    y = TILEH / 2;
                    break;
            }

            tiles[x, y] = new Tile(tiles[x, y].Rect.X, tiles[x, y].Rect.Y, TileType.background);
        }

        public void AddDoor(string dir)
        {
            //top, bottom, left, right
            int x = 0, y = 0;
            switch (dir)
            {
                case "top":
                    x = TILEW / 2;
                    y = 0;
                    break;
                case "bottom":
                    x = TILEW / 2;
                    y = TILEH - 1;
                    break;
                case "left":
                    x = 0;
                    y = TILEH / 2;
                    break;
                case "right":
                    x = TILEW - 1;
                    y = TILEH / 2;
                    break;
            }

            tiles[x, y] = new Tile(tiles[x, y].Rect.X, tiles[x, y].Rect.Y, TileType.door);
        }

        public List<Tile> GetTiles()
        {
            List<Tile> toReturn = new List<Tile>();

            for (int x = 0; x < TILEW; x++)
            {
                for (int y = 0; y < TILEH; y++)
                {
                    if (tiles[x, y].Type == TileType.solid ||
                        tiles[x, y].Type == TileType.rock ||
                        tiles[x, y].Type == TileType.door)
                    {
                        toReturn.Add(tiles[x, y]);
                    }
                }
            }
            return toReturn;
        }

        public List<Tile> SolidTiles
        {
            get
            {
                List<Tile> toReturn = new List<Tile>();
                for (int x = 0; x < TILEW; x++)
                {
                    for (int y = 0; y < TILEH; y++)
                    {
                        if (tiles[x, y].Type == TileType.solid ||
                        tiles[x, y].Type == TileType.rock ||
                        tiles[x, y].Type == TileType.door)
                        {
                            toReturn.Add(tiles[x, y]);
                        }
                    }
                }
                return toReturn;
            }
        }
    }
}
