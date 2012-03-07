using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Binding
{
    abstract class Enemy : Character
    {

        public Enemy(Vector2 pos, GameManager m)
            :base(pos, m)
        {
            ignorePlayerMovement = false;
            height = 30;
            width = 30;
            speed = new Vector2(0, 0);
            targetSource = new Vector2(0, 30);
            
            maxSpeed = 4.85f;
            maxAcc = 1.5f;
            maxDcc = 2f;
            dadt = 1.02f;

            range = 60;
            maxHealth = 3;
            health = 3;

            pauseUpdatingTimer = 40;
        }

        

        public override void Die()
        {
            //TODO play anim
            manager.ActorManager.Remove(this);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void  UpdateCollisions(List<Tile> tiles, List<Actor> actors, List<Player> players)
        {
            base.UpdateCollisions(tiles, actors, players);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
