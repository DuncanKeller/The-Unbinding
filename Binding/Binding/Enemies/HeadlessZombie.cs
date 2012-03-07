using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class HeadlessZombie : Enemy, ITouchDamage
    {
        Random rand;
        Vector2 dir;
        int movingTimer;
        bool spoutingBlood = false;

        public HeadlessZombie(Vector2 pos, GameManager m)
            : base(pos, m)
        {
            height = 40;
            width = 35;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);

            maxSpeed = 0.5f;
            maxAcc = 1.5f;
            maxDcc = .5f;
            dadt = 1.02f;

            maxHealth = 3;
            health = maxHealth;
            rand = GameConst.random;

            fireRate = 2;
        }

        public override void Update()
        {

            if (movingTimer > 0)
            {
                movingTimer--;
            }
            if (movingTimer == 0)
            {
                movingTimer = 30 + rand.Next(15);
                dir = ChangeDir();
            }

            if (movingTimer > 0)
            {
                speed += maxSpeed * dir;
            }
            else
            {
                if (speed.X < -maxDcc)
                {
                    speed.X += maxDcc;
                }
                else if (speed.X > maxDcc)
                {
                    speed.X -= maxDcc;
                }
                else
                {
                    speed.X = 0;
                }

                if (speed.Y < -maxDcc)
                {
                    speed.X += maxDcc;
                }
                else if (speed.Y > maxDcc)
                {
                    speed.Y -= maxDcc;
                }
                else
                {
                    speed.Y = 0;
                }
            }

            base.Update();
        }

        public Vector2 ChangeDir()
        {
            Vector2 returnVect = new Vector2((float)(rand.NextDouble() * 2) - 1,
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
            return returnVect;
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(GetHitRect().X, GetHitRect().Y, width, height), Color.Maroon);
            base.Draw(sb);
        }
    }
}
