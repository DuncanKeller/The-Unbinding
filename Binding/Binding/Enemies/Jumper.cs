using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Jumper : Enemy, ITouchDamage, IFlying
    {
        Random rand;
        Vector2 jumpDir = Vector2.Zero;
        Vector2 randDir = Vector2.Zero;
        Vector2 playerDir = Vector2.Zero;
        
        int MAX_JUMP = 25;
        int jumpTimer;
        bool inAir = false;

        public bool InAir
        {
            get { return inAir; }
            set { }
        }

        public Jumper(Vector2 pos, GameManager m)
            :base(pos, m)
        {
            height = 35;
            width = 40;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);
            
            maxSpeed = 5f;
            maxAcc = 1.0f;
            maxDcc = 2f;
            dadt = 1.02f;

            maxHealth = 2;
            health = maxHealth;
            rand = GameConst.random;
            jumpTimer = MAX_JUMP;
        }


        public Vector2 Jump(List<Tile> tiles)
        {
            Vector2 returnVect = Vector2.Zero;
            int failSafe = 10;

            while (returnVect == Vector2.Zero)
            {
                returnVect = new Vector2((float)(rand.NextDouble() * 2) - 1,
                (float)(rand.NextDouble() * 2) - 1);

                int dir = rand.Next(4);
                switch (dir)
                {
                    case 0:
                        returnVect.X = 1;
                        break;
                    case 1:
                        returnVect.Y = 1;
                        break;
                    case 2:
                        returnVect.X = -1;
                        break;
                    case 3:
                        returnVect.Y = -1;
                        break;
                }

                Rectangle testCollision = new Rectangle(
                    (int)(position.X + (maxSpeed * MAX_JUMP * returnVect.X)),
                    (int)(position.Y + (maxSpeed * MAX_JUMP * returnVect.Y)), 1, 1);
                foreach (Tile tile in tiles)
                {
                    if (testCollision.Intersects(tile.Rect))
                    {
                        returnVect = Vector2.Zero;
                        failSafe--;
                    }
                }

                if (failSafe == 0)
                {
                    returnVect = new Vector2(0, 0);
                }
            }

            return returnVect;
        }

        public override void Update()
        {
            if (!inAir)
            {
                jumpTimer--;
                if (jumpTimer == 0)
                {
                    if(playerDir != Vector2.Zero)
                    {
                        jumpDir = playerDir;
                    }
                    else
                    {
                        jumpDir = randDir;
                    }
                    jumpTimer = MAX_JUMP;
                    inAir = true;
                }
            }
            else
            {
                jumpTimer--;
                if (jumpTimer == 0)
                {
                    speed.X = 0;
                    speed.Y = 0;
                    jumpTimer = MAX_JUMP;
                    inAir = false;
                }
                else
                {

                    if (Math.Abs(speed.X) < maxSpeed)
                    {
                        speed.X += maxAcc * jumpDir.X;
                    }
                    if (Math.Abs(speed.Y) < maxSpeed)
                    {
                        speed.Y += maxAcc * jumpDir.Y;
                    }
                }
            }

            base.Update();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            double closestDist = 200;
            playerDir = Vector2.Zero;
            
            foreach (Player p in players)
            {
                double distX = p.GetRect().Center.X - GetRect().Center.X;
                double distY = p.GetRect().Center.Y - GetRect().Center.Y;
                double dist = Math.Abs(Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2)));
                if (dist < closestDist)
                {
                    playerDir = new Vector2((float)distX, (float)distY);
                    playerDir.Normalize();
                }
            }

            randDir = Jump(tiles);
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(GetRect().X, GetRect().Y, width, height), Color.Wheat);
            base.Draw(sb);
        }
    }
}
