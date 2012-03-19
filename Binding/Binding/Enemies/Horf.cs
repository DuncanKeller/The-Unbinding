using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Horf : Enemy, ITouchDamage, IShooting
    {
        Random rand;
        int sightRange;
        int timer;
        int fireRate;

        public bool InAir
        {
            get { return true; }
            set { }
        }

        public override Rectangle GetHitRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height - 15,
                width, height);
        }

        public Horf(Vector2 pos, GameManager m)
            : base(pos, m)
        {
            height = 40;
            width = 45;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, -15);

            maxSpeed = 2f;
            maxAcc = 1.5f;
            maxDcc = 2f;
            dadt = 1.12f;

            maxHealth = 3;
            health = maxHealth;
            rand = GameConst.random;

            sightRange = 250;
            fireRate = 2;
        }

        public override void Update()
        {

            if (timer > 0)
            {
                timer--;
            }

            base.Update();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);

            Vector2 chaseVect = new Vector2(0, 0);

            int closestDist = sightRange;
            foreach (Player p in players)
            {
                float distX = p.GetRect().Center.X - GetRect().Center.X;
                float distY = p.GetRect().Center.Y - GetRect().Center.Y;
                double dist = Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));
                if (dist < closestDist)
                {
                    Shoot(new Vector2(distX, distY));
                }
            }
        }

        public void Shoot(Vector2 dir)
        {
            if (timer == 0 && pauseUpdatingTimer == 0)
            {
                dir.Normalize();
                manager.BulletManager.Add(new Projectile(position + targetSource, new Vector2(speed.X + (12 * dir.X), speed.Y + (12 * dir.Y)), manager, 100, 1));
                timer = 100 - (10 * fireRate);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(GetRect().X, GetRect().Y, width, height), new Color(50, 50, 50, 50));
            sb.Draw(GameConst.blank, new Rectangle(GetHitRect().X, GetHitRect().Y, width, height), Color.Red);
            base.Draw(sb);
        }
    }
}
