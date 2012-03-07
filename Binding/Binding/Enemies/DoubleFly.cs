using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class DoubleFly : AggressiveFly, IFlying, ITouchDamage
    {
        Random rand;

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

        public DoubleFly(Vector2 pos, GameManager m)
            : base(pos, m)
        {
            height = 20;
            width = 30;
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
            

            base.Update();
        }

        public override void Die()
        {
            AggressiveFly fly1 = new AggressiveFly(position, manager);
            AggressiveFly fly2 = new AggressiveFly(position, manager);
            fly1.SpeedX = speed.X - 2f;
            fly2.SpeedX = speed.X + 2f;

            manager.ActorManager.Add(fly1);
            manager.ActorManager.Add(fly2);
            base.Die();
        }
        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
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
