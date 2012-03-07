using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    class Gish : Clotty, ITouchDamage, IShooting
    {
        Random rand;
        Vector2 dir;
        int movingTimer;
        int fireRate;


        public Gish(Vector2 pos, GameManager m)
            : base(pos, m)
        {
            height = 50;
            width = 50;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 0);

            maxSpeed = 1.3f;
            maxAcc = 1.5f;
            maxDcc = 2f;
            dadt = 1.02f;

            maxHealth = 5;
            health = maxHealth;
            rand = GameConst.random;

            fireRate = 2;
        }

        public override void Update()
        {

      
            base.Update();
        }

      

        public override void Shoot(Vector2 dir)
        {
            manager.BulletManager.Add(new Projectile(position,
                new Vector2(12, 12), manager, 800, 1));
            manager.BulletManager.Add(new Projectile(position,
                new Vector2(-12, -12), manager, 800, 1));
            manager.BulletManager.Add(new Projectile(position,
                new Vector2(-12, 12), manager, 800, 1));
            manager.BulletManager.Add(new Projectile(position,
                new Vector2(12, -12), manager, 800, 1));
            //shootingTimer = 100 - (10 * fireRate);
        }

        public override void UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(GameConst.blank, new Rectangle(GetHitRect().X, GetHitRect().Y, width, height), Color.DarkSlateGray);
            base.Draw(sb);
        }
    }
}
