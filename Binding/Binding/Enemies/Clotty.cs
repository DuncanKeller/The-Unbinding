using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Clotty : Enemy, ITouchDamage, IShooting
    {
        Random rand;
        Vector2 dir;
        int movingTimer;
        int fireRate;
        

        public Clotty(Vector2 pos, GameManager m)
            :base(pos, m)
        {
            height = 50;
            width = 50;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);
            
            maxSpeed = 1.3f;
            maxAcc = 1.5f;
            maxDcc = .5f;
            dadt = 1.02f;

            maxHealth = 5;
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
                movingTimer = 40 + rand.Next(20);
                dir = ChangeDir();
                Shoot(Vector2.Zero);
            }

            if (movingTimer > 15)
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

        public virtual void Shoot(Vector2 dir)
        {
            if (pauseUpdatingTimer == 0)
            {
                manager.BulletManager.Add(new Projectile(position,
               new Vector2(13, 0), manager, 800, 1));
                manager.BulletManager.Add(new Projectile(position,
                    new Vector2(-13, 0), manager, 800, 1));
                manager.BulletManager.Add(new Projectile(position,
                    new Vector2(0, 13), manager, 800, 1));
                manager.BulletManager.Add(new Projectile(position,
                    new Vector2(0, -13), manager, 800, 1));
            }
                //shootingTimer = 100 - (10 * fireRate);
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
