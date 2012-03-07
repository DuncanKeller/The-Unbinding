using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Binding
{
    class Character : Actor
    {
        protected int range;
        protected int damage;
        protected int fireRate;
        protected const int MAX_SHOOT_TIMER = 30;
        protected double shootTimer = MAX_SHOOT_TIMER;
        protected int health;
        protected int maxHealth;


        public Character(Vector2 pos, GameManager m)
            : base(pos, m)
        {
            ignorePlayerMovement = false;
            range = 60;

            damage = 1;
        }

        public void Hit(Projectile p)
        {
            health -= p.Damage;
            if (health <= 0)
            {
                Die();
            }
        }

        public void Hurt(int d)
        {
            health -= d;
            if (health <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {

        } 
        
        public void UpdateGameTime()
        {
            if (shootTimer < MAX_SHOOT_TIMER)
            {
                shootTimer += 1;
            }
        }

        public override void Update()
        {
            base.Update();
            UpdateGameTime();
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            foreach (Actor actor in actors)
            {
                if (this is Player)
                {
                    if (GetRect().Intersects(actor.GetRect()))
                    {
                        if (actor is ITouchDamage)
                        {
                            Hurt(1);
                        }
                    }
                }
            }

            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
