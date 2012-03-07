using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Map
    {
        public const int TILEW = 15;
        public const int TILEH = 9;
        public const int ROOMSW = 10;
        public const int ROOMSH = 6;
        const int LAYOUTS = 32;

        Room[,] rooms = new Room[ROOMSW, ROOMSH];
        List<Tile[,]> tilesets = new List<Tile[,]>();
        List<List<Enemy>> enemyConfigs = new List<List<Enemy>>();
        Room currRoom;
        Random rand;
        GameManager manager;


        public Room CurrentRoom
        {
            get { return currRoom; }
            set { currRoom = value; }
        }

        public Room North
        {
            get { if (currRoom.YPos > 0) { return rooms[currRoom.XPos, currRoom.YPos - 1]; } return null; }
        }

        public Room South
        {
            get { if (currRoom.YPos < ROOMSH - 1) { return rooms[currRoom.XPos, currRoom.YPos + 1]; } return null; }
        }

        public Room East
        {
            get { if (currRoom.XPos < ROOMSW - 1) { return rooms[currRoom.XPos + 1, currRoom.YPos]; } return null; }
        }

        public Room West
        {
            get { if (currRoom.XPos > 0) { return rooms[currRoom.XPos - 1, currRoom.YPos]; } return null; }
        }
        
        public Map(GameManager gm)
        {
            manager = gm;
            rand = new Random();      
        }

        public void Load()
        {
            LoadTiles();
            GenerateRooms(9);
        }

        public Room GetRoom(int x, int y)
        {
            return rooms[x, y];
        }

        public bool RoomExists(int x, int y)
        {
            if (x < 0 || y < 0 ||
                x >= ROOMSW || y >= ROOMSH)
            {
                return false;
            }
            if (rooms[x, y] == null)
            {
                return false;
            }
            return true;
        }

        public int Neightboring(int x, int y)
        {
            int count = 0;

            if (RoomExists(x - 1, y))
            {
                count++;
            }
            if (RoomExists(x + 1, y))
            {
                count++;
            }
            if (RoomExists(x, y - 1))
            {
                count++;
            }
            if (RoomExists(x, y + 1))
            {
                count++;
            }

            return count;
        }

        public void GenerateDoors()
        {
            for (int x = 0; x < ROOMSW; x++)
            {
                for (int y = 0; y < ROOMSH; y++)
                {
                    if (rooms[x, y] != null)
                    {
                        if (RoomExists(x - 1, y))
                        {
                            rooms[x, y].AddDoor("left");
                        }
                        if (RoomExists(x + 1, y))
                        {
                            rooms[x, y].AddDoor("right");
                        }
                        if (RoomExists(x, y - 1))
                        {
                            rooms[x, y].AddDoor("top");
                        }
                        if (RoomExists(x, y + 1))
                        {
                            rooms[x, y].AddDoor("bottom");
                        }
                    }
                }
            }
        }

        public void GenerateSpecials()
        {
            //boss, item, shop, arcade, secret
            List<Room> possibleSpecial = new List<Room>();
            
            for (int x = 0; x < ROOMSW; x++)
            {
                for (int y = 0; y < ROOMSH; y++)
                {
                    if (rooms[x, y] != null)
                    {
                        bool startingRoom = (x == ROOMSW / 2 && y == ROOMSH / 2);
                        if (Neightboring(x, y) == 1 &&
                            !startingRoom)
                        {
                            possibleSpecial.Add(rooms[x, y]);
                        }
                    }
                }
            }
        }

        public void GenerateRooms(int numRooms)
        {
            int currNumRooms = 1;

            rooms[ROOMSW / 2, ROOMSH / 2] = new Room(tilesets[0], ROOMSW / 2, ROOMSH / 2);

            while (currNumRooms < numRooms)
            {
                int randX = rand.Next(ROOMSW);
                int randY = rand.Next(ROOMSH);
                if (Neightboring(randX, randY) == 1
                    && rooms[randX, randY] == null)
                {
                    int layout = rand.Next(LAYOUTS);
                    rooms[randX, randY] = new Room(tilesets[layout], randX, randY);
                    foreach (Enemy e in enemyConfigs[layout])
                    {
                        Actor a = (Actor)e;
                        a.Translate((randX * 800) + 0 * Tile.WIDTH, (randY * 480) + 0 * Tile.HEIGHT);
                        a.CurrentRoom = rooms[randX, randY];
                        manager.ActorManager.Add(a);
                    }
                    currNumRooms++;
                }
            }

            currRoom = rooms[ROOMSW / 2, ROOMSH / 2];

            GenerateDoors();
            GenerateSpecials();
            GenerateSpecials();
        }

        public void MoveRoom(int x, int y)
        {
            currRoom = rooms[x,y];
        }

        public int NumEnemies
        {
            get
            {
                int count = 0;
                foreach (Actor a in manager.ActorManager.Items)
                {
                    if (a is Enemy)
                    {
                        if (a.CurrentRoom == CurrentRoom)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        public void LoadTiles()
        {
            for (int i = 0; i <= LAYOUTS; i++)
            {
                List<Enemy> enemies = new List<Enemy>();
                StreamReader sr = new StreamReader(TitleContainer.OpenStream("Content\\Tilesets\\1\\"+ i +".txt"));
                Tile[,] tempTiles = new Tile[TILEW, TILEH];
                string line = "";
                int heightIndex = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    for (int x = 0; x < line.Length; x++)
                    {
                        Tile tile = null;
                        if (line[x] == '|' || line[x] == '?')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.solid);
                        }
                        else if (line[x] == '-')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                        }
                        else if (line[x] == '#' || line[x] == '@')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.rock);
                        }
                        else if (line[x] == 'a')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new Fly(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'b')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new AggressiveFly(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'c')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new DoubleFly(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'd')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new ShootingFly(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'e')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new FireFly(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'f')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new Horf(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT)), 
                                manager));
                        }
                        else if (line[x] == 'g')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            //manager.ActorManager.Add(new FireFly(new Vector2((x * 53) + (TILEW / 2), (heightIndex * 53) + (TILEW / 2)),
                            //   manager));
                        }
                        else if (line[x] == 'h')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new HeadlessZombie(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'i')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new Jumper(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'j')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            //manager.ActorManager.Add(new FireFly(new Vector2((x * 53) + (TILEW / 2), (heightIndex * 53) + (TILEW / 2)),
                            //    manager));
                        }
                        else if (line[x] == 'k')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new Clotty(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'l')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            enemies.Add(new Gish(new Vector2((x * Tile.WIDTH) + (Tile.WIDTH), ((heightIndex + 1) * Tile.HEIGHT) + (Tile.HEIGHT)),
                                manager));
                        }
                        else if (line[x] == 'm')
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                            //enemies.Add(new FireFly(new Vector2((x * 53) + (TILEW / 2), (heightIndex * 53) + (TILEW / 2)),
                            //    manager));
                        }
                        else
                        {
                            tile = new Tile(x * Tile.WIDTH, heightIndex * Tile.HEIGHT, TileType.background);
                        }
                        
                        tempTiles[x, heightIndex] = tile;
                    }
                    heightIndex++;
                }

                enemyConfigs.Add(enemies);
                tilesets.Add(tempTiles);
            }
        }

        public List<Tile> GetTiles()
        {
            return currRoom.GetTiles();
        }

        public List<Tile> SolidTiles
        {
            get
            {
                return currRoom.SolidTiles;
            }
        }

        public void OpenDoors()
        {
            currRoom.RemoveDoors(this);
            if (North != null)
            {
                North.RemoveDoor("bottom");
            }
            if (South != null)
            {
                South.RemoveDoor("top");
            }
            if (East != null)
            {
                East.RemoveDoor("left");
            }
            if (West != null)
            {
                West.RemoveDoor("right");
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in GetTiles())
            {
                tile.Draw(sb);
            }

            if (North != null)
            {
                foreach (Tile tile in North.GetTiles())
                {
                    tile.Draw(sb);
                }
            }

            if (South != null)
            {
                foreach (Tile tile in South.GetTiles())
                {
                    tile.Draw(sb);
                }
            }

            if (East != null)
            {
                foreach (Tile tile in East.GetTiles())
                {
                    tile.Draw(sb);
                }
            }

            if (West != null)
            {
                foreach (Tile tile in West.GetTiles())
                {
                    tile.Draw(sb);
                }
            }
        }
        
    }
}
