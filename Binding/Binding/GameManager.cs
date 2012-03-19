using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class GameManager
    {
        Camera cam;
        List<Player> players = new List<Player>();
        Vector2 camDestination;

        Map map;

        ActorManager<Actor> actorManager;
        ActorManager<Projectile> bulletManager;

        public ActorManager<Actor> ActorManager
        {
            get { return actorManager; }
        }

        public ActorManager<Projectile> BulletManager
        {
            get { return bulletManager; }
        }

        public Camera Cam
        {
            get { return cam; }
        }

        public Vector2 CamDestination
        {
            get { return camDestination; }
            set { camDestination = value; }
        }

        public Map Map
        {
            get { return map; }
        }
        

        public GameManager(GraphicsDeviceManager graphics)
        {
            map = new Map(this);
            actorManager = new ActorManager<Actor>(this);
            bulletManager = new ActorManager<Projectile>(this);

            map.Load();

            cam = new Camera(graphics);
            cam.pos.X += (map.CurrentRoom.XPos * 800) + 400;
            cam.pos.Y += (map.CurrentRoom.YPos * 480) + 180;
            camDestination = cam.pos;

            players.Add(new Player(new Vector2((map.CurrentRoom.XPos * 800) + 100, (map.CurrentRoom.YPos * 480) + 100), this));
        }

        public void MoveRoom(Room room)
        {
            map.CurrentRoom = room;
        }

        public void MoveCam(int x, int y)
        {
            cam.pos.X += x;
            cam.pos.Y += y;
        }

        public void MoveCamDest(int x, int y)
        {
            camDestination.X += x;
            camDestination.Y += y;
        }

        public void AddBomb(Bomb b)
        {
            actorManager.Add(b);
        }

        public void UpdateCamera()
        {
            Vector2 distance = new Vector2(
                camDestination.X - cam.pos.X,
                camDestination.Y - cam.pos.Y);
            Vector2 normDist = new Vector2(
                camDestination.X - cam.pos.X,
                camDestination.Y - cam.pos.Y);
            if (!(normDist.X == 0 &&
                normDist.Y == 0))
            {
                normDist.Normalize();
            }
            distance /= cam.Dampening;
            if (distance.X != 0)
            {
                distance.X = Math.Abs(distance.X * cam.Speed);
            }
            if (distance.Y != 0)
            {
                distance.Y = Math.Abs(distance.Y * cam.Speed);
            }
            float moveX = distance.X * normDist.X;
            float moveY = distance.Y * normDist.Y;

            if (moveX != float.NaN
                && moveX != 0)
            {
                cam.pos.X += moveX;
            }
            if (moveY != float.NaN
                && moveY != 0)
            {
                cam.pos.Y += moveY;
            }

            if (Math.Abs(cam.pos.X - camDestination.X) < 2)
            {
                cam.pos.X = camDestination.X;
            }
            if (Math.Abs(cam.pos.Y - camDestination.Y) < 2)
            {
                cam.pos.Y = camDestination.Y;
            }
        }

        public void Update()
        {
            List<Entity> toDispose = new List<Entity>();
            
            foreach (Player p in players)
            {
                 p.Update();
                 p.UpdateCollisions(map.SolidTiles, actorManager.Items, players);           
            }

            bulletManager.Update(map.SolidTiles, actorManager.Items, players);
            actorManager.Update(map.SolidTiles, actorManager.Items, players);

            if (map.NumEnemies == 0
                && map.CurrentRoom.Compelted == false)
            {
                map.OpenDoors();
                foreach (Player p in players)
                {
                    if (p.CurrentRoom == map.CurrentRoom)
                    {
                        p.ChargeItem();
                    }
                }
                map.CurrentRoom.Compelted = true;
            }

            UpdateCamera();
        }

        public void Draw(SpriteBatch sb)
        {
            map.Draw(sb);

            foreach (Player p in players)
            {
                p.Draw(sb);
            }

            bulletManager.Draw(sb);
            actorManager.Draw(sb);
        }
        
    }
}
