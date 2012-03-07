using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class AggressiveFly : Enemy, IFlying, ITouchDamage
    {
        Random rand;
        Player closestPlayer = null;

        public bool InAir
        {
            get { return true; }
            set { }
        }

        public override Rectangle GetHitRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height - 40,
                width, height);
        }

        public AggressiveFly(Vector2 pos, GameManager m)
            : base(pos, m)
        {
            height = 20;
            width = 20;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);

            maxSpeed = 5f;
            maxAcc = 1.5f;
            maxDcc = 2f;
            dadt = 1.02f;

            maxHealth = 2;
            health = maxHealth;
            rand = GameConst.random;
        }

        public override void Update()
        {
            float x = (float)(rand.NextDouble() * 4) - (4 / 2);
            float y = (float)(rand.NextDouble() * 4) - (4 / 2);

            x /= 7;
            y /= 7;

            Vector2 chaseVect = new Vector2(0, 0);

            if (closestPlayer != null)
            {
                chaseVect.X = closestPlayer.GetRect().Center.X - GetRect().Center.X;
                chaseVect.Y = closestPlayer.GetRect().Center.Y - GetRect().Center.Y;

                chaseVect.Normalize();

                x = (chaseVect.X / 15) + (float)((rand.NextDouble() - 0.5) / 3);
                y = (chaseVect.Y / 15) + (float)((rand.NextDouble() - 0.5) / 3);
            }

            speed.X += x;
            speed.Y += y;

            base.Update();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            
            double closestDist = 500;
            foreach (Player p in players)
            {
                double distX = Math.Abs(GetRect().Center.X - p.GetRect().Center.X);
                double distY = Math.Abs(GetRect().Center.Y - p.GetRect().Center.Y);
                double dist = Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));
                if (dist < closestDist)
                {
                    closestPlayer = p;
                }
            }
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(GetRect().X, GetRect().Y, width, height), new Color(50, 50, 50, 50));
            sb.Draw(GameConst.blank, new Rectangle(GetRect().X, GetRect().Y - 40, width, height), Color.Red);
            base.Draw(sb);
        }
    }
}
