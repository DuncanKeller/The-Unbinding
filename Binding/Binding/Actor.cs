using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
     class Actor : Entity
    {
        protected Vector2 targetSource;
        protected Vector2 speed;
        protected float xDcc;
        protected float yDcc;
        protected float maxAcc;
        protected float maxDcc;
        protected float maxSpeed;
        protected float dadt;

        Room currRoom;
        protected int pauseUpdatingTimer = 0;
         

        private List<Tile> collidingTiles = new List<Tile>();

        GameTime time;
        protected bool ignorePlayerMovement = true;

        public Room CurrentRoom
        {
            get { return currRoom; }
            set { currRoom = value; }
        }

        public float SpeedX
        {
            get { return speed.X; }
            set { speed.X = value; }
        }

        public float SpeedY
        {
            get { return speed.Y; }
            set { speed.Y = value; }
        }

        public virtual Rectangle GetRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height,
            width, height); 
        }

        public virtual Rectangle GetHitRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height,
            width, height);
        }


        public Actor(Vector2 pos, GameManager m)
            :base(pos, m)
        {
            height = 30;
            width = 30;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);
            
            maxSpeed = 4.85f;
            maxAcc = 1.5f;
            maxDcc = 2f;
            dadt = 1.02f;

            

            time = new GameTime();
        }

        public void Translate(int x, int y)
        {
            position.X += x;
            position.Y += y;
        }

        public override void Update()
        {
            if (pauseUpdatingTimer == 0)
            {
                if (speed.X > maxSpeed)
                {
                    speed.X = maxSpeed;
                }
                if (speed.X < -maxSpeed)
                {
                    speed.X = -maxSpeed;
                }

                if (speed.Y > maxSpeed)
                {
                    speed.Y = maxSpeed;
                }
                if (speed.Y < -maxSpeed)
                {
                    speed.Y = -maxSpeed;
                }

                if (!(this is Player))
                {
                    if (Math.Abs(speed.X) > 0)
                    {
                        speed.X /= dadt;
                    }
                    if (Math.Abs(speed.Y) > 0)
                    {
                        speed.Y /= dadt;
                    }
                }

                position.X += speed.X;
                position.Y += speed.Y;
            }
            else
            {
                if (pauseUpdatingTimer > 0)
                {
                    pauseUpdatingTimer--;
                }
            }
        }

        public bool CollidingTileDir(Vector2 dir)
        {
            foreach (Tile tile in collidingTiles)
            {
                if (dir.X > 0
                    && tile.Rect.Center.X > this.GetRect().Center.X )
                {
                    return true;
                }
                if (dir.X < 0
                    && tile.Rect.Center.X < this.GetRect().Center.X)
                {
                    return true;
                }
                if (dir.Y > 0
                    && tile.Rect.Center.Y > this.GetRect().Center.Y)
                {
                    return true;
                }
                if (dir.Y < 0
                    && tile.Rect.Center.Y < this.GetRect().Center.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            if (CurrentRoom == manager.Map.CurrentRoom)
            {
                collidingTiles.Clear();

                foreach (Tile tile in tiles)
                {
                    if (tile.Rect.Intersects(GetRect()))
                    {
                        bool flying = false;
                        if (this is IFlying)
                        {
                            flying = (this as IFlying).InAir;
                        }

                        if (!flying ||
                            tile.Type == TileType.solid)
                        {
                            collidingTiles.Add(tile);
                            Vector2 responseVect = new Vector2(0, 0);
                            responseVect.X = GetRect().Center.X - tile.Rect.Center.X;
                            responseVect.Y = GetRect().Center.Y - tile.Rect.Center.Y;

                            Rectangle intersection = Intersection(GetRect(), tile.Rect);
                            #region ROZ
                            //hi duncan 
                            //HI ROZ
                            //HOW ARE YOU?
                            //Pretty good. How about yourself?
                            //ALSO PRETTY GOOD!!! :DDDD IM WRITTING IN UR CODE AGAIN!
                            // No wai :o
                            //YEAH WAI! IS THIS THE SAME CODE AS BEFORE?
                            //NOPE :(
                            //AWWWWW :( HW?
                            //No, just for fun
                            //OOOH! THATS WHY I CAN COMMENT YUR CODE!
                            //YUP
                            //YAAAAAAAAAY!! OKAY I HAS TO GO NAOS
                            //BYE ROZ :D
                            //BAI DUNCAAN! <333333
                            #endregion
                            if (Math.Abs(responseVect.X) > Math.Abs(responseVect.Y))
                            {
                                if (responseVect.X > 0)
                                {
                                    position.X += intersection.Width;
                                }
                                else
                                {
                                    position.X -= intersection.Width;
                                }
                            }
                            else
                            {
                                if (responseVect.Y > 0)
                                {
                                    position.Y += intersection.Height;
                                }
                                else
                                {
                                    position.Y -= intersection.Height;
                                }
                            }
                        }
                    }
                }
            }

            Vector2 collisionVector = new Vector2(0, 0);

            foreach (Actor actor in actors)
            {
                if (GetRect().Intersects(actor.GetRect())
                     && !actor.ignorePlayerMovement)
                {
                    if (this != actor)
                    {
                        collisionVector.X += GetRect().Center.X - actor.GetRect().Center.X;
                        collisionVector.Y += GetRect().Center.Y - actor.GetRect().Center.Y;
                    }
                }
            }

            foreach (Player player in players)
            {
                if (GetRect().Intersects(player.GetRect())
                    && !ignorePlayerMovement)
                {
                    if (this != player)
                    {
                        collisionVector.X +=  GetRect().Center.X - player.GetRect().Center.X;
                        collisionVector.Y +=  GetRect().Center.Y - player.GetRect().Center.Y;
                    }
                }
            }

            speed.X += collisionVector.X / 15;
            speed.Y += collisionVector.Y / 15;
        }

        private Rectangle Intersection(Rectangle r1, Rectangle r2)
        {
            int x = 0, y = 0, w = 0, h = 0;

            if (r1.X < r2.X)
            {
                x = r2.Left;
                w = r1.Right - x;
            }
            else
            {
                x = r1.Left;
                w = r2.Right - x;
            }

            if (r1.Y < r2.Y)
            {
                y = r2.Top;
                h = r1.Bottom - y;
            }
            else
            {
                y = r1.Top;
                h = r2.Bottom - y;
            }

            return new Rectangle(x, y, w, h);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(GameConst.blank, rect, Color.Blue);
        }
    }
}
