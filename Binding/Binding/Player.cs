using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Binding
{
    class Player : Character
    {
        KeyboardState keyState;
        KeyboardState oldKeyState;
        int bombs = 10;
        int keys = 3;

        public Player(Vector2 pos, GameManager m)
            :base(pos, m)
        {
            width = 50;
            height = 60;
            dadt = 1.2f;
            maxHealth = 6;
            health = maxHealth;
            targetSource = new Vector2(0, 10);
            CurrentRoom = manager.Map.CurrentRoom;
        }

        public override Rectangle GetRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - (height / 2),
                    width, height / 2);
        }

        public override Rectangle GetHitRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - (height),
                    width, height);
        }


        public void UpdateCamera()
        {
            CurrentRoom = manager.Map.CurrentRoom;
            if (manager.CamDestination.X == manager.Cam.pos.X &&
                manager.CamDestination.Y == manager.Cam.pos.Y)
            {
                if (GetRect().Left < manager.Cam.pos.X - 400 + (Tile.WIDTH / 2))
                {
                    manager.MoveCamDest(-800, 0);
                    manager.MoveRoom(manager.Map.West);
                    position.X -= ((Tile.WIDTH * 2) + (GetRect().Width / 2));
                    speed = new Vector2(0, 0);
                    manager.Map.GenerateDoors();
                }
                else if (GetRect().Right > manager.Cam.pos.X + 400 - (Tile.WIDTH/ 2))
                {
                    manager.MoveCamDest(800, 0);
                    manager.MoveRoom(manager.Map.East);
                    position.X += ((Tile.WIDTH * 2) + (GetRect().Width / 2));
                    speed = new Vector2(0, 0);
                    manager.Map.GenerateDoors();
                }
                else if (GetRect().Top < manager.Cam.pos.Y - 240 + 60 + (Tile.HEIGHT / 2))
                {
                    manager.MoveCamDest(0, -480);
                    manager.MoveRoom(manager.Map.North);
                    position.Y -= ((Tile.HEIGHT * 2) + (GetRect().Height / 2));
                    speed = new Vector2(0, 0);
                    manager.Map.GenerateDoors();
                }
                else if (GetRect().Bottom > manager.Cam.pos.Y + 240 + 60 - (Tile.HEIGHT / 2))
                {
                    manager.MoveCamDest(0, 480);
                    manager.MoveRoom(manager.Map.South);
                    position.Y += ((Tile.HEIGHT * 2) + (GetRect().Height / 2));
                    speed = new Vector2(0, 0);
                    manager.Map.GenerateDoors();
                }
            }
        }


        public override void Update()
        {
            
            keyState = Keyboard.GetState();

            bool holdingXKey = false;
            bool holdingYKey = false;

            if (keyState.IsKeyDown(Keys.W)
                )
            {
                speed.Y -= maxAcc;

                holdingYKey = true;
                yDcc = maxDcc;
            }
            if (keyState.IsKeyDown(Keys.S)
                )
            {
                speed.Y += maxAcc;

                holdingYKey = true;
                yDcc = maxDcc;
            }
            if (keyState.IsKeyDown(Keys.A)
                 )
            {
                speed.X -= maxAcc;

                holdingXKey = true;
                xDcc = maxDcc;
            }
            if (keyState.IsKeyDown(Keys.D)
                 )
            {
                speed.X += maxAcc;

                holdingXKey = true;
                xDcc = maxDcc;
            }

            
            if (!holdingXKey)
            {
                if (Math.Abs(xDcc) > 0)
                {
                    speed.X /= dadt;
                }
            }

            if (!holdingYKey)
            {
                if (Math.Abs(yDcc) > 0)
                {
                    speed.Y /= dadt;
                }
            }

            //shootin
            if (shootTimer == MAX_SHOOT_TIMER)
            {
                if (keyState.IsKeyDown(Keys.Up))
                {
                    manager.BulletManager.Add(new Projectile(position - targetSource,
                        new Vector2(speed.X, speed.Y), new Vector2(0, -1),
                        manager, range, damage));
                    shootTimer = 0;
                }
                else if (keyState.IsKeyDown(Keys.Down))
                {
                    manager.BulletManager.Add(new Projectile(position - targetSource,
                        new Vector2(speed.X, speed.Y), new Vector2(0, 1),
                        manager, range, damage));
                    shootTimer = 0;
                }
                else if (keyState.IsKeyDown(Keys.Left))
                {
                    manager.BulletManager.Add(new Projectile(position - targetSource,
                        new Vector2(speed.X, speed.Y), new Vector2(-1, 0),
                        manager, range, damage));
                    shootTimer = 0;
                }
                else if (keyState.IsKeyDown(Keys.Right))
                {
                    manager.BulletManager.Add(new Projectile(position - targetSource,
                        new Vector2(speed.X, speed.Y), new Vector2(1, 0),
                        manager, range, damage));
                    shootTimer = 0;
                }
            }

            if (keyState.IsKeyDown(Keys.E) &&
                oldKeyState.IsKeyUp(Keys.E))
            {
                if (bombs > 0)
                {
                    Bomb bomb = new Bomb(new Vector2(position.X, position.Y), manager, this);
                    bomb.CurrentRoom = CurrentRoom;
                    manager.AddBomb(bomb);
                    bombs--;
                }
            }

            UpdateCamera();

            oldKeyState = keyState;

            base.Update();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);

            foreach (Actor actor in actors)
            {
                if (actor is Key)
                {
                    manager.ActorManager.Remove(actor);
                    keys++;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.Draw(GameConst.blank, new Rectangle((int)position.X - (width / 2), (int)position.Y - height,
                width, height), Color.DarkBlue);
        }
        
    }
}
