using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Fly : Enemy, IFlying
    {
        Random rand;
        Vector2 direction = new Vector2();

        public bool InAir
        {
            get { return true; }
            set { }
        }

        public override Rectangle GetHitRect()
        {
            return new Rectangle((int)position.X - (width / 2), (int)position.Y - height - 30,
                width, height);
        }

        public Fly(Vector2 pos, GameManager m)
            :base(pos, m)
        {
            height = 20;
            width = 20;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);
            
            maxSpeed = 5f;
            maxAcc = 1.5f;
            maxDcc = 2f;
            dadt = 1.02f;

            maxHealth = 1;
            health = maxHealth;
            rand = GameConst.random;
        }

        public override void Update()
        {
            float x = (float)(rand.NextDouble() * 4) - (4 / 2);
            float y = (float)(rand.NextDouble() * 4) - (4 / 2);

            x /= 7;
            y /= 7;

            speed.X += x;
            speed.Y += y;

            base.Update();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(GetRect().X, GetRect().Y, width, height), new Color(50, 50, 50, 50));
            sb.Draw(GameConst.blank, new Rectangle(GetHitRect().X, GetHitRect().Y, width, height), Color.Red);
            base.Draw(sb);
        }
    }
}
